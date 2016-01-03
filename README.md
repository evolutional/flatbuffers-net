# Description

This library provides utilities to interact with [Google FlatBuffers] using the idioms and APIs familiar to .NET developers.

# Motivation

The [FlatBuffers] project contains [flatc], a schema compiler and code generator that emits a C# API to interact with your data in FlatBuffer format. This 'canonical' API has been designed for maximum performance and minimal allocations - a core goal of the FlatBuffers project.

However there are times that, as a .NET developer, you wish to use FlatBuffers as a data format and are willing to sacrifice the performance and low-allocation characteristics of the generated API in favour of an API that follows a more familiar, .NET-centric pattern.

This project is inspired by Marc Gravell's [Protobuf-net], which achieves this goal admirably.

Using the features provided by this project, it should be possible to achieve a *code-first* workflow, whereby your data contracts are created in .NET and a [fbs] schema can be created from them. You can then generate the APIs for other languages from this schema using `flatc`.

# Quickstart - Serialization

## Simple Struct Serialization Example
 
The simplest place to start using the library is to declare a `struct` in C#. This will serialize the object as  `struct` in FlatBuffer format (eg: fields are inline).

```cs
	public struct TestStruct
	{
		public int IntProp { get; set; }
	}
```

We now need to use the `FlatBuffersSerializer` to serialize an instance of `TestStruct` into a `byte[]` in FlatBuffer format.

```cs
	var obj = new TestStruct() { IntProp = 42 };	
	var buffer = new byte[32];
	var serializer = new FlatBuffersSerializer();	
	var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);
```

This will copy the serialized data to your byte array at the offset you specified (in this case zero).

Now you can deserialize the data in the buffer into a new .NET object of `TestStruct` type.

```cs
	var newObj = serializer.Deserialize<TestStruct>(buffer, 0, bytesWritten);
```

## Simple Table Serialization Example

Declaring a .NET `class` will (by default) treat the type as a `table` in FlatBuffer format. This is an object that can contain empty fields and reference types (such as `strings`).

```cs
	public class TestTable
	{
		public int IntProp { get; set; }
		public string Name { get; set; }
	}
```

As before, we use the `FlatBuffersSerializer` to serialize an instance of this object to a byte array in FlatBuffer format.

```cs
	var obj = new TestTable() { IntProp = 42, Name = "An example name" };	
	var buffer = new byte[64];
	var serializer = new FlatBuffersSerializer();	
	var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);
```

This will take care of serializing the string `"An example name"` to the buffer and emitting a `table` that references it.
 
Deserialization follows the same approach as before.

```cs
	var newObj = serializer.Deserialize<TestTable>(buffer, 0, bytesWritten);
```

## Vector Serialization

The `FlatBuffersSerializer` supports serialization of types derived from `ICollection` (such as `Array`, `IList` and the generic `List<>` types). As a result it is possible to serialize .NET objects with definitions similar to these:

```cs
    public class TestTableWithArrays
    {
        public int[] IntArray{ get; set; }
        public List<int> IntList { get; set; }
    }
```

These are (de)serialized in the same way as the previous examples.


# Quickstart - fbs Schema Generation

## Simple Struct Schema Generation Example

Given the `struct` we declared earlier, it is possible to use `FlatBuffersSchemaWriter` to emit a fbs schema for the type. In this example, we'll emit to a `StringBuilder` using a `StringWriter`.

```cs
	var sb = new StringBuilder();
	using (var sw = new StringWriter(sb))
	{
		var schemaWriter = new FlatBuffersSchemaWriter(sw);
		schemaWriter.Write<TestStruct1>();
	}
```

The `StringBuffer` will now contain the following text:

```
	struct TestStruct1 {
		IntProp:int;
		ByteProp:ubyte;
		ShortProp:short;
	}
```

# Type mapping

This project uses the following type mappings between .NET base types and FlatBuffers.

|.NET Type		| FlatBuffers Type	| Size (bits)	|
|---------------|-------------------|---------------|
| `sbyte`		| `byte`			| 8				|
| `byte`		| `ubyte`			| 8				|
| `bool`		| `bool`			| 8				|
| `short`		| `short`			| 16			|
| `ushort`		| `ushort`			| 16			|
| `int`			| `int`				| 32			|
| `uint`		| `uint`			| 32			|
| `long`		| `long`			| 64			|
| `ulong`		| `ulong`			| 64			|
| `float`		| `float`			| 32			|
| `double`		| `double`			| 64			|
| `enum`		| `enum`			| User-specified|
| `string`		| `string`			| Varies		|
| `struct`		| `struct`			| Varies		|
| `class`		| `table`			| Varies		|


## Vectors
Vectors (fbs schema: `[type]`) are represented via .NET arrays (eg: `T[]`) or `ICollection` based types such as `List<T>`.

## Strings
Strings in .NET are stored natively in `UTF-16`; however the FlatBuffers spec requires that strings are written in `UTF-8` format. The serialization process will handle the string encoding process. 


# Library Components

## FlatBuffersSerializer

A serializer that is capable of serializing .NET objects to/from FlatBuffers format.

## FlatBuffersSchemaWriter

Generates a schema definition from your .NET objects (in [fbs] format) for use with the [flatc] code generator.

## TypeModel / TypeModelRegistry

This is a set of utilities which use Reflection to map .NET types to a FlatBuffers type definition. These will not be used directly by the majority of users.

# Building

Build the core solution `FlatBuffers.Serialization.sln` in Visual Studio 2013/2015. 

The solution contains 3 projects:

* `FlatBuffers.csproj` - the .NET port of [FlatBuffers] referenced in the flatbuffers submodule. It currently uses the 'safe' configuration.
* `FlatBuffers.Serialization.csproj` - the main FlatBuffers-net code
* `FlatBuffers.Serialization.Tests.csproj` - an MS Test project containing the unit & integration tests

This project has only been built in Visual Studio 2013/2015 on Windows x64. No other platforms (eg: Mono/Unity/Xamarin) have been verified.

# Testing

## FlatBuffer Verification

There are no guarantees that the contents of the buffer emitted by the `FlatBuffersSerializer` will be exactly the same as that of the canonical output, however the buffers should be *compatible* with each other.

As a result, we are currently using the [test oracle] approach to verify that the serialization routines emit a buffer that can be read by the canonical C# code that `flatc` will emit. Likewise, the canonical code is used to emit a buffer to be read by the deserialization routines in this library.


# Status

This library is a work in progress, currently in an early stage of development and is not recommended for use in production applications.

It has not been optimized, will have missing/incomplete features and will most likely contain undiscovered bugs.
 
There is currently very little documentation other than the unit tests and this README.

## Planned Features

* Attributes to control reflection (eg: ordering, default values, required fields, etc)
* Serialization/Deserialization of non-`struct`/`table` as root
* User-created typemodels (eg: non-reflection)
* Richer fbs schema writer features (namespaces, includes, dependency ordering, etc)

For a more detailed roadmap, please see the [issues](https://github.com/evolutional/flatbuffers-net/issues) page.

# Acknowledgements

This project uses parts of [Google FlatBuffers]'s .NET port to create and interact with FlatBuffers.

Marc Gravell's [Protobuf-net] is a key inspiration for the motivation behind this project.



[test oracle]: https://en.wikipedia.org/wiki/Oracle_(software_testing)
[Google FlatBuffers]: http://google.github.io/flatbuffers/
[FlatBuffers]: http://google.github.io/flatbuffers/
[flatc]: http://google.github.io/flatbuffers/md__compiler.html
[fbs]: https://google.github.io/flatbuffers/md__schemas.html
[Protobuf-net]: https://github.com/mgravell/protobuf-net