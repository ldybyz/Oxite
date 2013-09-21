
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;
using System.ServiceModel;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcMessageEncodingBindingElement : MessageEncodingBindingElement
    {
        // Fields
        private int maxReadPoolSize;
        private int maxWritePoolSize;
        private XmlDictionaryReaderQuotas readerQuotas;

        // Methods
        public XmlRpcMessageEncodingBindingElement() : this(MessageVersion.None,Encoding.UTF8)
        {
        }

        private XmlRpcMessageEncodingBindingElement(XmlRpcMessageEncodingBindingElement elementToBeCloned) : base(elementToBeCloned)
        {
            this.maxReadPoolSize = elementToBeCloned.maxReadPoolSize;
            this.maxWritePoolSize = elementToBeCloned.maxWritePoolSize;
            this.readerQuotas = new XmlDictionaryReaderQuotas();
            elementToBeCloned.readerQuotas.CopyTo(this.readerQuotas);
        }

        public XmlRpcMessageEncodingBindingElement(MessageVersion messageVersion, Encoding writeEncoding)
        {
            this.maxReadPoolSize = 0x40;
            this.maxWritePoolSize = 0x10;
            this.readerQuotas = new XmlDictionaryReaderQuotas();
            XmlRpcMessageEncoder.GetDefaultReaderQuotas().CopyTo(this.readerQuotas);
        }

        public override BindingElement Clone()
        {
            return new XmlRpcMessageEncodingBindingElement(this);
        }

         public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context) 
        {
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context) 
        {
            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new XmlRpcMessageEncoderFactory(this.MaxReadPoolSize, this.MaxWritePoolSize, this.ReaderQuotas);
        }

        public override T GetProperty<T>(BindingContext context) 
        {
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
            {
                return this.readerQuotas as T;
            }
            if (typeof(T) == typeof(MessageVersion))
            {
                return MessageVersion as T;
            }
            return base.GetProperty<T>(context);
        }
        
        //CFV:internal bool xIsMatch(BindingElement bindingElement)
        //{
        //    if (!base.IsMatch(bindingElement))
        //    {
        //        return false;
        //    }
        //    XmlRpcMessageEncodingBindingElement messageEncodingElement = bindingElement as XmlRpcMessageEncodingBindingElement;
        //    if (messageEncodingElement == null)
        //    {
        //        return false;
        //    }
        //    if (this.maxReadPoolSize != messageEncodingElement.MaxReadPoolSize)
        //    {
        //        return false;
        //    }
        //    if (this.maxWritePoolSize != messageEncodingElement.MaxWritePoolSize)
        //    {
        //        return false;
        //    }
        //    if (this.readerQuotas.MaxStringContentLength != messageEncodingElement.ReaderQuotas.MaxStringContentLength)
        //    {
        //        return false;
        //    }
        //    if (this.readerQuotas.MaxArrayLength != messageEncodingElement.ReaderQuotas.MaxArrayLength)
        //    {
        //        return false;
        //    }
        //    if (this.readerQuotas.MaxBytesPerRead != messageEncodingElement.ReaderQuotas.MaxBytesPerRead)
        //    {
        //        return false;
        //    }
        //    if (this.readerQuotas.MaxDepth != messageEncodingElement.ReaderQuotas.MaxDepth)
        //    {
        //        return false;
        //    }
        //    if (this.readerQuotas.MaxNameTableCharCount != messageEncodingElement.ReaderQuotas.MaxNameTableCharCount)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        
        // Properties
        public int MaxReadPoolSize
        {
            get
            {
                return this.maxReadPoolSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "");
                }
                this.maxReadPoolSize = value;
            }
        }

        public int MaxWritePoolSize
        {
            get
            {
                return this.maxWritePoolSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "");
                }
                this.maxWritePoolSize = value;
            }
        }

        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get
            {
                return this.readerQuotas;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return MessageVersion.None;
            }
            set
            {
                if (value != MessageVersion.None)
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

