using ChatBox.Models;

using Stylet;

namespace ChatBox.ViewModels;
public class ChatViewModel : Screen
{
    private string title;

    public string Title
    {
        get => title;
        set => SetAndNotify(ref title, value);
    }
    private readonly BindableCollection<Chat> _chats;
    public BindableCollection<Chat> Chats => _chats;

    public ChatViewModel()
    {
        title = "新话题";
        _chats =
        [
            new Chat
            {
                Body = "c# 中 JsonSerializer 如何序列化派生类属性",
                Dock = Avalonia.Controls.Dock.Right
            },
            new Chat {
                Body = """
                如果你想使用官方自带的序列化实现，可以使用 .NET Framework 内置的 DataContractJsonSerializer 类。这个类提供了将对象序列化为 JSON 格式和将 JSON 反序列化为对象的功能。

                下面是一个示例代码，演示如何使用 DataContractJsonSerializer 序列化和反序列化派生类属性：

                ```csharp
                using System;
                using System.IO;
                using System.Runtime.Serialization;
                using System.Runtime.Serialization.Json;

                [DataContract]
                public class Animal
                {
                    [DataMember]
                    public string Name { get; set; }

                    [DataMember]
                    public int Age { get; set; }
                }

                [DataContract]
                public class Dog : Animal
                {
                    [DataMember]
                    public bool IsBarking { get; set; }
                }

                public static class Program
                {
                    static void Main()
                    {
                        var dog = new Dog { Name = "Fido", Age = 3, IsBarking = true };

                        // 序列化
                        var serializer = new DataContractJsonSerializer(typeof(Dog));
                        using (var stream = new MemoryStream())
                        {
                            serializer.WriteObject(stream, dog);
                            stream.Position = 0;
                            using (var reader = new StreamReader(stream))
                            {
                                string json = reader.ReadToEnd();
                                Console.WriteLine(json);
                            }
                        }

                        // 反序列化
                        using (var stream = new MemoryStream())
                        {
                            using (var writer = new StreamWriter(stream))
                            {
                                writer.Write("{\"Name\":\"Charlie\",\"Age\":5,\"IsBarking\":false}");
                                writer.Flush();
                                stream.Position = 0;

                                var deserializedDog = (Dog)serializer.ReadObject(stream);
                                Console.WriteLine(deserializedDog.Name);
                                Console.WriteLine(deserializedDog.Age);
                                Console.WriteLine(deserializedDog.IsBarking);
                            }
                        }
                    }
                }
                ```

                在上面的示例中，我们使用 DataContract 和 DataMember 属性对 Animal 和 Dog 类进行标记，以指定要序列化/反序列化的属性。然后，我们实例化 DataContractJsonSerializer 类，并使用 WriteObject() 方法将对象序列化为 JSON 字符串。使用 ReadObject() 方法可以将 JSON 反序列化为对象。

                当我们运行程序时，它会输出以下内容：

                ```
                {"Age":3,"IsBarking":true,"Name":"Fido"}
                Charlie
                5
                False
                ```

                请注意，DataCo
                """
            },
        ];
    }

    public void SendMessage()
    {
    }
}
