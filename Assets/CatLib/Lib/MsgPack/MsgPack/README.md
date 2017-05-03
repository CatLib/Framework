#Version MsgPack.Cli.0.9.0-beta2
now use unity/xxx.dll
if you use .net2.0 or not .net2.0 sub,please use unity-full/xxx.dll

As of MesseagePack for CLI 0.5, Xamarin and Unity (including Android and iOS) are supported.

This wiki page describe how to use MessagePack for CLI in their environment with some restrictions.

Overview

MesseagePack for CLI core API can run on Xamarin and Unity. In addition, you can use serializer (MessagePackSerializer<T>) in their platform by generating them in advance.

Rationale and History

iOS does NOT permit any runtime code generation from JIT to IL generation (OK, latest Xamarin supports ExpressionTree interpreter, but it is not so fast now). So, because MessagePack for CLI serialization strongly depends on runtime code generation (via System.Reflection.Emit and/or System.Linq.Expression), it must fail when serializer object generation. As of 0.5, MessagePack for CLI introduces new serializer generation stack which uses CodeDOM (System.CodeDom) which spawns serializers as source code. By generating serializers source code in advance and building with them, you can get AOTed serializers.

How to Use in Xamarin

MessagePack for CLI can use in form of a NuGet package.

Refer MessagePack for CLI library

Get NuGet package and install it to Xamarin project. It contains runtime library of MessagePack for CLI. Note that it does not include any code related to runtime code generation.
Generate serializers in advance

Note As of 0.5.1, you can use reflection based serializers, so you does not have to do following instruction to work with MessagePack for CLI. But it is recommended to generate serializers in advance because reflection based serializers are not so fast. Note You must use generated serializers when you serialize value types (including enums) in Unity3D.

Separate serializer generation project from application project. This project is normal desktop project and refers application library project which contains serialization targets. For example, you can use NUnit testing project.
Get NuGet package and install latest MessagePack for CLI package to the project.
Invoke SerializerGenerator API in the project.
Add generated serializers to the project as you like.
For example, you can write NUnit test code to generate serializers as follows:

// using System.Linq;

[TestFixture]
public class SerializerGenerator
{
    [Test]
    public void GenerateSerializers()
    {
        var applicationLibraryAssembly = typeof( ApplicationLibraryClass ).Assembly;
        SerializerGenerator.GenerateCode(
            new SerializerCodeGenerationConfiguration
            {
                Namespace = "Example.MessagePackSerializers",
                OutputDirectory = "../../GeneratedSerializers",
                EnumSerializationMethod = EnumSerializationMethod.ByName, // You can tweak it to use ByUnderlyingValue as you like.
                IsRecursive = true, // Set depenendent serializers are also generated.
                PreferReflectionBasedSerializer = false, // Set true if you want to use reflection based collection serializer, false if you want to get generated collection serializers.
                SerializationMethod = SerializationMethod.Array // You tweak it to generate 'map' based serializers.
            },
            applicationLibraryAssembly.GetTypes().Where( type =>
                /* ...you can filter types to be serialized by their namespace, custom attributes, etc... */
            )
        );
    }
}
As you imagine, you can embed above code in MSBuild pipeline or custom build action to automate serializer code generation.

Using Generated Serializers

You must setup serialization context to use generated serializers as following:

(Optional) Create custom SerializationContext and store it as you like.
Register serializer instances with SerializationContext.RegisterSerializer repeatedly. Target serialization context is custom context created above or default context via SerializationContext.Default. Note that you must register all serializers for object tree except some primitives or FCL(BCL) types.
How to Use in Unity3D using DLL(Recommended)

Download latest zip file from "github release page"(https://github.com/msgpack/msgpack-cli/releases).
Unzip *.zip file and goto bin/Unity3D.Full or bin/Unity3D and find MsgPack.dll file. Note that some tool (including archiver utility in MacOS X) cannot handle Windows style subdirectory correctly, so if you get invalid file which contains back solidous in the name, please try unzipping from terminal (e.g. unzip).
Import the MsgPack.dll to your asset library.
How to Use in Unity3D using Source

If you don't like DLL style asset, you can import MessagePack for CLI source code to the assets as follows.

Preparation

If you have not been have Unity project, create it.
Remember the root directory of the Assets directory.
Check out source tree.
Import Core Library

Open command prompt or terminal on tools/mpu/bin.
Run following command to copy source tree for Unity asset:
mpu.exe -l -o <OUTPUT_DIR>
mono mpu.exe -l -o <OUTPUT_DIR>
Import generated source tree to your Assets library.
If you want to build "unity3d-full" source code, you must define MSGPACK_UNITY_FULL compilation constant in project settings.

Generate Serializers

Run following command line to generate serializer source code. Note that you can specify --include and --exclude options to filter target types with regular expression toward their type full name (e.g. "System.DateTime").

If you built target types as assembly, you can use following:
mpu.exe -s -a -n MyProduct.MsgPackSerializers -o /path-to-asset-root/MsgPackSerializers /path/to/assembly.dll
Or (normal case) you can use following:
mpu.exe -s -n MyProduct.MsgPackSerializers -o /path-to-asset-root/MsgPackSerializers /path/to/sources/Some1.cs /path/to/sources/Some2.cs ...
When you import MsgPack as source code style instead of dll (assembly) style, you also specify --internal option for the mpu.exe.

Limitations of Xamarin

Recent Xamarin runtime works well for generics even if they use value type.
Linker still tend to cause runtime error, so it is not recommended to use linker. But you can tweak hard to annotate with custom attributes of Xamarin. See Xamarin documents/blogs for details.
Some generics (such as List<int>.Enumerator) causes ExecutionEngineException. It is limitations of Mono (for iOS). MessagePack for CLI 0.6 has many workarounds for them, but issues might exist yet.
Xamarin iOS does not support dynamic code generation due to iOS limitation, so MessagePack for CLI uses reflection based implementation, they are slower than code generation implementation. You can pre-generate serializers with SerializerGenerator API (or you can do via mpu.exe command line tool too) to avoid this limitation.
Some 'dry-run' might be required for the AOT to tell that some types/members are required in runtime. See test/MsgPack.UnitTest.Xamarin.iOS/Main.cs for example.
Limitations of Unity

ALL limitations for Xamarin are also applied to Unity.
Unity has additional limitation related to generics, so best option is avoiding generics as possible. MessagePack for CLI 0.6 has many workarounds for them, but issues may exist yet. ** As of 0.6.5, you can use MessagePackSerializer.PrepareType<T>() for value types. It should solve ExecutionEngineException related to SerializationContext.GetSerializer<T>(object) or BoxingGenericEqualityComparer issues of Dictionary<TKey, TValue>.
Some generics (such as IEnumerable<int>) causes ExecutionEngineException. It is limitations of Unity (for iOS). Consider using boxing. As of 0.6.0, MessagePack for CLI uses boxing and reflections heavily for Unity build to avoid this problem.
Nested value type generics like KeyValuePair<string, DateTimeOffset> are not supported. To enable them, you must register custom serializer. See test/MsgPack.UnitTest.Unity3D/Serialization/AotWorkarounds.cs for example.
You can use "corlib only" subset. You must use unity3d\MsgPack.dll in the distributed .zip file. Note that the build cannot refer System.dll and System.Core.dll, so some built-in serializers for FCL collections are disabled.
You can use the build for "full" subset, it is unity3d-full/MsgPack.dll.
Stripping is not supported.
IL2CPP supporting is in progress... Please help us via reporting issues and submitting pull-requests.