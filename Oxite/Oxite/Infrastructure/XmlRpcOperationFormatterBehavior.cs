
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;
using System.Reflection;
using System.Runtime.Serialization;
using System.IO;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcOperationFormatterBehavior : IOperationBehavior, IWsdlExportExtension
    {
        DataContractSerializerOperationBehavior _dcs;
        XmlSerializerOperationBehavior _xcs;

        public XmlRpcOperationFormatterBehavior(DataContractSerializerOperationBehavior dcs, XmlSerializerOperationBehavior xcs)
        {
            _dcs = dcs;
            _xcs = xcs;            
        }

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            clientOperation.Formatter = new XmlRpcOperationFormatter(operationDescription);
            clientOperation.SerializeRequest = clientOperation.DeserializeReply = true;
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            if (dispatchOperation.Parent.ChannelDispatcher.BindingName.EndsWith("XmlRpcHttpBinding"))
            {
                dispatchOperation.Formatter = new XmlRpcOperationFormatter(operationDescription);
                dispatchOperation.DeserializeRequest = dispatchOperation.SerializeReply = true;
            }
            else if (_dcs != null)
            {
                ((IOperationBehavior)_dcs).ApplyDispatchBehavior(operationDescription, dispatchOperation);
            }
            else if (_xcs != null)
            {
                ((IOperationBehavior)_xcs).ApplyDispatchBehavior(operationDescription, dispatchOperation);
            }
        }

        public void ValidateXmlRpcType(Type type)
        {
            if (type != typeof(string) &&
                 type != typeof(bool) &&
                 type != typeof(int) &&
                 type != typeof(DateTime) &&
                 type != typeof(TimeSpan) &&
                 type != typeof(double) &&
                 type != typeof(float)&&
                 type != typeof(byte[]) && 
                 type != typeof(Stream))
            {
                if (type.IsArray)
                {
                    ValidateXmlRpcType(type.GetElementType());
                }
                else if (type.IsDefined(typeof(DataContractAttribute), false))
                {
                    foreach (MemberInfo member in type.GetMembers())
                    {
                        if (member.IsDefined(typeof(DataMemberAttribute), true))
                        {
                            if (member is PropertyInfo)
                            {
                                ValidateXmlRpcType(((PropertyInfo)member).PropertyType);
                            }
                            else if (member is FieldInfo)
                            {
                                ValidateXmlRpcType(((FieldInfo)member).FieldType);
                            }
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Validate(OperationDescription operationDescription)
        {
            List<ParameterInfo> parameters;
            ParameterInfo returnValue;

            if (operationDescription.SyncMethod != null)
            {
                parameters = new List<ParameterInfo>(operationDescription.SyncMethod.GetParameters());
                returnValue = operationDescription.SyncMethod.ReturnParameter;
            }
            else if (operationDescription.BeginMethod != null &&
                      operationDescription.EndMethod != null)
            {
                parameters = new List<ParameterInfo>(operationDescription.BeginMethod.GetParameters());
                parameters.RemoveRange(parameters.Count - 2, 2);
                returnValue = operationDescription.EndMethod.ReturnParameter;
            }
            else
            {
                throw new InvalidOperationException();
            }
            foreach (ParameterInfo param in parameters)
            {
                if (param.IsOut)
                {
                    throw new InvalidOperationException();
                }
                ValidateXmlRpcType(param.ParameterType);
            }
            if (returnValue != null)
            {
                ValidateXmlRpcType(returnValue.ParameterType);
            }            
        }

        #region IWsdlExportExtension Members

        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
            if (_dcs != null && _dcs is IWsdlExportExtension)
            {
                ((IWsdlExportExtension)_dcs).ExportContract(exporter, context);
            }
            else if (_xcs != null && _xcs is IWsdlExportExtension)
            {
                ((IWsdlExportExtension)_xcs).ExportContract(exporter, context);
            }
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            if (_dcs != null && _dcs is IWsdlExportExtension)
            {
                ((IWsdlExportExtension)_dcs).ExportEndpoint(exporter, context);
            }
            else if (_xcs != null && _xcs is IWsdlExportExtension)
            {
                ((IWsdlExportExtension)_xcs).ExportEndpoint(exporter, context);
            }
        }

        #endregion
    }
}
