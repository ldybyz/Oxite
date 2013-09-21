
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.Net.Mime;
using System.Xml;
using System.IO;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcMessageEncoder : MessageEncoder
    {
        private ContentType _contentType = new ContentType(MediaTypeNames.Text.Xml);
        private XmlRpcMessageEncoderFactory _factory;

        public XmlRpcMessageEncoder(XmlRpcMessageEncoderFactory factory)
        {
            _factory = factory;
        }

        public override string ContentType
        {
            get { return _contentType.ToString(); }
        }

        public override string MediaType
        {
            get { return _contentType.MediaType; }
        }

        public override MessageVersion MessageVersion
        {
            get { return _factory.MessageVersion; }
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            MemoryStream stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue, contentType);
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream stream = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(stream,Encoding.UTF8,false);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            int messageLength = (int)stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }

        public override Message ReadMessage(System.IO.Stream stream, int maxSizeOfHeaders, string contentType)
        {
            XmlDictionaryReader messageReader = XmlDictionaryReader.CreateTextReader(stream, _factory.ReaderQuotas);
            string methodName;

            while ( messageReader.Read() )
            {
                if (messageReader.IsStartElement(XmlRpcProtocol.MethodCall))
                {
                    messageReader.ReadStartElement();
                    messageReader.MoveToContent();
                    if (!messageReader.IsStartElement(XmlRpcProtocol.MethodName))
                    {
                        throw new XmlRpcFormatException("Missing method name");
                    }
                    else
                    {
                        messageReader.ReadStartElement();
                        messageReader.MoveToContent();
                        if (messageReader.NodeType == XmlNodeType.Text)
                        {
                            methodName = messageReader.ReadString();
                            messageReader.ReadEndElement();
                        }
                        else
                        {
                            throw new XmlRpcFormatException("Missing method name");
                        }
                        if (messageReader.IsStartElement(XmlRpcProtocol.Params))
                        {
                            return new XmlRpcMessage(methodName, messageReader);
                        }
                        else
                        {
                            messageReader.Close();
                            return new XmlRpcMessage(methodName);
                        }
                    }
                }
                else if (messageReader.IsStartElement(XmlRpcProtocol.MethodResponse))
                {
                    messageReader.ReadStartElement();
                    messageReader.MoveToContent();
                    if (messageReader.IsStartElement(XmlRpcProtocol.Params))
                    {
                        return new XmlRpcMessage(messageReader);
                    }
                    else
                    {
                        messageReader.Close();
                        return new XmlRpcMessage();
                    }
                }
            }
            throw new XmlRpcFormatException("Invalid Message");
        }

        public override void WriteMessage(Message message, System.IO.Stream stream)
        {
            XmlDictionaryWriter messageWriter = XmlDictionaryWriter.CreateTextWriter(stream);
            message.WriteMessage(messageWriter);
        }

        public static XmlDictionaryReaderQuotas GetDefaultReaderQuotas()
        {
            XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
            quotas.MaxDepth = 0x20;
            quotas.MaxStringContentLength = 0x2000;
            quotas.MaxArrayLength = 0x4000;
            quotas.MaxBytesPerRead = 0x1000;
            quotas.MaxNameTableCharCount = 0x4000;
            return quotas;
        }
    }

    public class XmlRpcMessageEncoderFactory : MessageEncoderFactory
    {
        XmlRpcMessageEncoder _encoder;
        int _maxReadPoolSize;
        int _maxWritePoolSize; 
        XmlDictionaryReaderQuotas _readerQuotas;

        public XmlRpcMessageEncoderFactory(int maxReadPoolSize, int maxWritePoolSize, XmlDictionaryReaderQuotas readerQuotas)
        {
            _encoder = new XmlRpcMessageEncoder(this);
            _maxReadPoolSize = maxReadPoolSize;
            _maxWritePoolSize = maxWritePoolSize;
            _readerQuotas = readerQuotas;
        }

        public override MessageEncoder Encoder
        {
            get { return _encoder; }
        }

        public override MessageVersion MessageVersion
        {
            get { return MessageVersion.None; }
        }

        public int MaxReadPoolSize
        {
            get { return _maxReadPoolSize; }
        }

        public int MaxWritePoolSize
        {
            get { return _maxWritePoolSize; }
        }

        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get { return _readerQuotas; }
        }

        

	}
}
