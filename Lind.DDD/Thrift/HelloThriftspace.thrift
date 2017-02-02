namespace csharp HelloThriftspace

exception Xception {
  1: i32 errorCode,
  2: string message
}

service HelloThrift{
  void HelloWorld() throws (1:Xception ex),
  string GetData(1:i32 uid)
}