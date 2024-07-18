using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Generator
{
    public sealed class Program
    {
        const string LIBS1 = "./libraries\\";
        const string LIBS2 = "./libraries/";
        const string LIBS3 = "./libraries";

        static void Main(string[] args)
        {
            var ms = new MemoryStream();
            var writer = new Utf8JsonWriter(ms);

            writer.WriteStartArray();

            int items = Directory.GetDirectories("./libraries").Length;
            int last = 1;
            foreach (string dir in Directory.GetDirectories("./libraries"))
            {
                string lib = dir.Split('\\')[1];
                Console.WriteLine($"Processing assembly {last++} out of {items}, please wait.. (Library: {lib}).");
                string actualDir = string.Concat(dir, "/src/");
                string asmName = CleanUpObjectName(dir);
                foreach (string file in Directory
                    .EnumerateFiles(actualDir, "*.*", SearchOption.AllDirectories)
                    .Where(f => f.EndsWith(".cs")))
                {
                    // C# files only
                    var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
                    string fileName = file.Replace("\\", "/").Split('/').Last();
                    string folderName = string.Join("\\", file.Split('\\').Reverse().Skip(1).Reverse());
                    ProcessSrcFile(syntaxTree, fileName, folderName, ref writer);
                }
            }

            writer.WriteEndArray();
            writer.Flush();

            string result = Encoding.UTF8.GetString(ms.ToArray());
            Console.WriteLine("Formatting JSON and writing...");
            File.AppendAllText("result.json", result);
            Console.WriteLine("Formatting...");
            string formattedJson = JsonSerializer.Serialize(
                JsonDocument.Parse(result), new JsonSerializerOptions { WriteIndented = true });
            File.AppendAllText("resultformatted.json", formattedJson);
            Console.WriteLine("Done");
        }

        private static string CleanUpObjectName(string dir)
        {
            return dir.StartsWith(LIBS1) ? dir[..LIBS1.Length]
                    : dir.StartsWith(LIBS2) ? dir[..LIBS2.Length]
                    : dir.StartsWith(LIBS3) ? dir[..LIBS3.Length]
                    : dir;
        }

        private static void ProcessSrcFile(SyntaxTree syntaxTree, string fileName, string folderName, ref Utf8JsonWriter writer)
        {
            foreach (var type in syntaxTree.GetRoot().DescendantNodes())
            {
                switch (type)
                {
                    case NamespaceDeclarationSyntax namespaceDeclaration:
                        string ownerName = namespaceDeclaration.Name.ToString();
                        foreach (var nestedType in namespaceDeclaration.DescendantNodes())
                        {
                            switch (nestedType)
                            {
                                case ClassDeclarationSyntax classDeclaration:
                                    ProcessType(classDeclaration, fileName, folderName, ref writer, ownerName);
                                    break;
                                case StructDeclarationSyntax structDeclaration:
                                    ProcessType(structDeclaration, fileName, folderName, ref writer, ownerName);
                                    break;
                                case DelegateDeclarationSyntax delegateDeclaration:
                                    ProcessType(delegateDeclaration, fileName, folderName, ref writer, ownerName);
                                    break;
                                case EnumDeclarationSyntax enumDeclaration:
                                    ProcessType(enumDeclaration, fileName, folderName, ref writer, ownerName);
                                    break;
                                case InterfaceDeclarationSyntax interfaceDeclaration:
                                    ProcessType(interfaceDeclaration, fileName, folderName, ref writer, ownerName);
                                    break;
                                case EnumMemberDeclarationSyntax recordDeclaration:
                                    ProcessType(recordDeclaration, fileName, folderName, ref writer, ownerName);
                                    break;
                            }
                        }
                        break;
                    case FileScopedNamespaceDeclarationSyntax fileScopedNamespace:
                        string ownerName2 = fileScopedNamespace.Name.ToString();
                        foreach (var nestedType in fileScopedNamespace.DescendantNodes())
                        {
                            switch (nestedType)
                            {
                                case ClassDeclarationSyntax classDeclaration:
                                    ProcessType(classDeclaration, fileName, folderName, ref writer, ownerName2);
                                    break;
                                case StructDeclarationSyntax structDeclaration:
                                    ProcessType(structDeclaration, fileName, folderName, ref writer, ownerName2);
                                    break;
                                case DelegateDeclarationSyntax delegateDeclaration:
                                    ProcessType(delegateDeclaration, fileName, folderName, ref writer, ownerName2);
                                    break;
                                case EnumDeclarationSyntax enumDeclaration:
                                    ProcessType(enumDeclaration, fileName, folderName, ref writer, ownerName2);
                                    break;
                                case InterfaceDeclarationSyntax interfaceDeclaration:
                                    ProcessType(interfaceDeclaration, fileName, folderName, ref writer, ownerName2);
                                    break;
                                case EnumMemberDeclarationSyntax recordDeclaration:
                                    ProcessType(recordDeclaration, fileName, folderName, ref writer, ownerName2);
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ProcessType(SyntaxNode node, string fileName, string folderName, ref Utf8JsonWriter writer, string ownerName)
        {
            folderName = folderName.Replace("./", "").Replace("\\", "/")
                .Replace("/libraries/libraries", "/libraries")
                .Replace("libraries", "libraries/")
                .Replace("//", "/");
            switch (node)
            {
                case ClassDeclarationSyntax _1:
                    writer.WriteStartObject();
                    writer.WriteString("name", $"{ownerName}.{_1.Identifier.Text}");
                    writer.WriteString("url", $"https://raw.githubusercontent.com/dotnet/runtime/main/src/{folderName}/{fileName}");
                    writer.WriteEndObject();
                    break;
                case StructDeclarationSyntax _1:
                    writer.WriteStartObject();
                    writer.WriteString("name", $"{ownerName}.{_1.Identifier.Text}");
                    writer.WriteString("url", $"https://raw.githubusercontent.com/dotnet/runtime/main/src/{folderName}/{fileName}");
                    writer.WriteEndObject();
                    break;
                case EnumDeclarationSyntax _1:
                    writer.WriteStartObject();
                    writer.WriteString("name", $"{ownerName}.{_1.Identifier.Text}");
                    writer.WriteString("url", $"https://raw.githubusercontent.com/dotnet/runtime/main/src/{folderName}/{fileName}");
                    writer.WriteEndObject();
                    break;
                case DelegateDeclarationSyntax _1:
                    writer.WriteStartObject();
                    writer.WriteString("name", $"{ownerName}.{_1.Identifier.Text}");
                    writer.WriteString("url", $"https://raw.githubusercontent.com/dotnet/runtime/main/src/{folderName}/{fileName}");
                    writer.WriteEndObject();
                    break;
                case InterfaceDeclarationSyntax _1:
                    writer.WriteStartObject();
                    writer.WriteString("name", $"{ownerName}.{_1.Identifier.Text}");
                    writer.WriteString("url", $"https://raw.githubusercontent.com/dotnet/runtime/main/src/{folderName}/{fileName}");
                    writer.WriteEndObject();
                    break;
                case RecordDeclarationSyntax _1:
                    writer.WriteStartObject();
                    writer.WriteString("name", $"{ownerName}.{_1.Identifier.Text}");
                    writer.WriteString("url", $"https://raw.githubusercontent.com/dotnet/runtime/main/src/{folderName}/{fileName}");
                    writer.WriteEndObject();
                    break;
            }
        }
    }
}
