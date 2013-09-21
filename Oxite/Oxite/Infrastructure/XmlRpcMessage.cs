
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcMessage : Message 
    {
        private MessageHeaders _headers;
        private MessageProperties _properties;
        private bool _isFault;
        private XmlDictionaryReader _bodyReader;

        public XmlRpcMessage()
        {
            _bodyReader = null;
            _headers = new MessageHeaders(MessageVersion.None);
            _properties = new MessageProperties();
        }

        public XmlRpcMessage( string methodName, XmlDictionaryReader paramsSection ):this()
        {
            _bodyReader = paramsSection;
            _bodyReader.MoveToContent();
            _isFault = false;
            _properties.Add("XmlRpcMethodName",methodName);
        }

        public XmlRpcMessage(string methodName)
            : this()
        {
            _bodyReader = null;
            _isFault = false;
            _properties.Add("XmlRpcMethodName", methodName);
        }

        public XmlRpcMessage(XmlDictionaryReader paramsSection):this()
        {
            _bodyReader = paramsSection;
            _bodyReader.MoveToContent();
            _isFault = false;
        }

        public XmlRpcMessage(MessageFault fault):this()
        {
            _isFault = true;
            _bodyReader = XmlRpcDataContractSerializationHelper.CreateFaultReader(fault);
            _bodyReader.MoveToContent();
        }

        public override MessageHeaders Headers
        {
            get { return _headers; }
        }

        public override MessageProperties Properties
        {
            get { return _properties; }
        }

        public bool IsXmlRpcMethodCall
        {
            get { return _properties.ContainsKey("XmlRpcMethodName"); }
        }

        public override bool IsFault
        {
            get
            {
                return _isFault;
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return _bodyReader == null;
            }
        }

        public override MessageVersion Version
        {
            get { return MessageVersion.None; }
        }

        protected override void  OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            if (!IsEmpty)
            {
                writer.WriteNode(_bodyReader, true);
            }
        }

        protected override void OnWriteMessage(XmlDictionaryWriter writer)
        {
            OnWriteStartEnvelope(writer);
            if (IsXmlRpcMethodCall)
            {
                writer.WriteStartElement(XmlRpcProtocol.MethodName);
                writer.WriteString((string)_properties["XmlRpcMethodName"]);
                writer.WriteEndElement();
            }
            OnWriteBodyContents(writer);
            writer.WriteEndElement();
        }

        protected override void OnWriteStartEnvelope(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement(IsXmlRpcMethodCall ? XmlRpcProtocol.MethodCall : XmlRpcProtocol.MethodResponse);
        }

        protected override void OnWriteStartBody(XmlDictionaryWriter writer)
        {
            throw new NotSupportedException();
        }

        protected override void OnWriteStartHeaders(XmlDictionaryWriter writer)
        {
            throw new NotSupportedException();
        }

        protected override MessageBuffer OnCreateBufferedCopy(int maxBufferSize)
        {
            return base.OnCreateBufferedCopy(maxBufferSize);
        }

        
    }
}
