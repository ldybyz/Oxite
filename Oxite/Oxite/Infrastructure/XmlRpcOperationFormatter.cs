
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Reflection;
using System.ServiceModel.Description;
using System.Xml;
using System.IO;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    class XmlRpcOperationFormatter : IDispatchMessageFormatter, IClientMessageFormatter
    {
        OperationDescription _operationDescription;
        List<ParameterInfo> _parameterInfo;
        ParameterInfo _returnParameter;

        public XmlRpcOperationFormatter(OperationDescription operationDescription)
        {
            _operationDescription = operationDescription;
            if (_operationDescription.SyncMethod != null)
            {
                _parameterInfo = new List<ParameterInfo>(_operationDescription.SyncMethod.GetParameters());
                _returnParameter = _operationDescription.SyncMethod.ReturnParameter;
            }
            else if (_operationDescription.BeginMethod != null && _operationDescription.EndMethod != null)
            {
                _parameterInfo = new List<ParameterInfo>(_operationDescription.BeginMethod.GetParameters());
                _returnParameter = _operationDescription.EndMethod.ReturnParameter;
                _parameterInfo.RemoveRange(_parameterInfo.Count - 2, 2);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        #region IClientMessageFormatter Members

        object IClientMessageFormatter.DeserializeReply(System.ServiceModel.Channels.Message message, object[] parameters)
        {
            object returnValue = null;

            XmlDictionaryReader bodyContentReader = message.GetReaderAtBodyContents();
            bodyContentReader.ReadStartElement(XmlRpcProtocol.Params);
            if (bodyContentReader.IsStartElement(XmlRpcProtocol.Param))
            {
                bodyContentReader.ReadStartElement();
                if (bodyContentReader.IsStartElement(XmlRpcProtocol.Value))
                {
                    bodyContentReader.ReadStartElement();
                    if (bodyContentReader.NodeType == XmlNodeType.Text)
                    {
                        returnValue = bodyContentReader.ReadContentAs(_returnParameter.ParameterType, null);
                    }
                    else
                    {
                        returnValue = XmlRpcDataContractSerializationHelper.Deserialize(bodyContentReader, _returnParameter.ParameterType);
                    }
                    bodyContentReader.ReadEndElement();
                }
                bodyContentReader.ReadEndElement();
            }
            bodyContentReader.Close();
            return returnValue;
        }

        System.ServiceModel.Channels.Message IClientMessageFormatter.SerializeRequest(System.ServiceModel.Channels.MessageVersion messageVersion, object[] parameters)
        {
            MemoryStream memStream = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(memStream);

            writer.WriteStartElement(XmlRpcProtocol.Params);
            foreach (object param in parameters)
            {
                writer.WriteStartElement(XmlRpcProtocol.Param);
                writer.WriteStartElement(XmlRpcProtocol.Value);
                XmlRpcDataContractSerializationHelper.Serialize(writer, param);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Flush();

            memStream.Position = 0;
            XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
            XmlRpcMessage xmlRpcMessage = new XmlRpcMessage(_operationDescription.Messages[0].Action,XmlDictionaryReader.CreateTextReader(memStream, quotas));
            return xmlRpcMessage;
        }

        #endregion

        #region IDispatchMessageFormatter Members

        void IDispatchMessageFormatter.DeserializeRequest(System.ServiceModel.Channels.Message message, object[] parameters)
        {
            int paramCounter = 0;

            XmlDictionaryReader bodyContentReader = message.GetReaderAtBodyContents();
            bodyContentReader.ReadStartElement(XmlRpcProtocol.Params);
            if (parameters.Length > 0)
            {
                while (bodyContentReader.IsStartElement(XmlRpcProtocol.Param) && paramCounter < parameters.Length)
                {
                    bodyContentReader.ReadStartElement();
                    if (bodyContentReader.IsStartElement(XmlRpcProtocol.Value))
                    {
                        bodyContentReader.ReadStartElement();
                        if (bodyContentReader.NodeType == XmlNodeType.Text && !string.IsNullOrEmpty(bodyContentReader.Value.Trim()))
                        {
                            parameters[paramCounter] = bodyContentReader.ReadContentAs(_parameterInfo[paramCounter].ParameterType, null);
                        }
                        else
                        {
                            parameters[paramCounter] = XmlRpcDataContractSerializationHelper.Deserialize(bodyContentReader, _parameterInfo[paramCounter].ParameterType);
                        }
                        bodyContentReader.ReadEndElement();
                    }
                    bodyContentReader.ReadEndElement();
                    bodyContentReader.MoveToContent();
                    paramCounter++;
                }
            }
            bodyContentReader.ReadEndElement();
            bodyContentReader.Close();
        }

        System.ServiceModel.Channels.Message IDispatchMessageFormatter.SerializeReply(System.ServiceModel.Channels.MessageVersion messageVersion, object[] parameters, object result)
        {
            MemoryStream memStream = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(memStream);

            writer.WriteStartElement(XmlRpcProtocol.Params);
            writer.WriteStartElement(XmlRpcProtocol.Param);
            writer.WriteStartElement(XmlRpcProtocol.Value);
            XmlRpcDataContractSerializationHelper.Serialize(writer, result);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();

            memStream.Position = 0;
            XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
            XmlRpcMessage xmlRpcMessage = new XmlRpcMessage(XmlDictionaryReader.CreateTextReader(memStream, quotas));
            return xmlRpcMessage;
        }

        #endregion
    }
}
