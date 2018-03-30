namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    public static class JsonRpcErrorCodes {

        public const int ParseError = -32700;
        public const int InvalidRequest = -32600;
        public const int MethodNotFound = -32601;
        public const int InvalidParams = -32602;
        public const int InternalError = -32603;
        // ReSharper disable once InconsistentNaming
        public const int ServerError_Start = -32099;
        // ReSharper disable once InconsistentNaming
        public const int ServerError_End = -32000;
        public const int ServerNotInitialized = ServerError_Start + 97;
        public const int Unknown = -32001;

    }
}
