[![Build and Test CodeGen](https://github.com/Golle/longtail/actions/workflows/build.yml/badge.svg)](https://github.com/Golle/longtail/actions/workflows/build.yml)
[![Run longtail tests](https://github.com/Golle/longtail/actions/workflows/longtail.yml/badge.svg)](https://github.com/Golle/longtail/actions/workflows/longtail.yml)

## Longtail C# Bindings

This project generates cross platform C# bindings for Dan Engelbrecht longtail library. [Longtail](https://github.com/DanEngelbrecht/longtail).

## Current status

**Implemented**
* Generates bindings for all structs, enums, functions, function pointers
* Supports .NET5.0 and .NET 6.0 on platforms Linux, MacOS and Windows x64
* Automatic documentation for function pointers with the original names of parameters

**Working on**
* Copy the comments from the API functions and generate C# comments in the LongtailLibrary.cs file
* Nuget package for the low level bindings (Unsafe)
* Managed abstrations (hide all unsafe code behin easy to use managed functions and classes)
* More tests for both CodeGen and Longtail

## Project structure

#### src/CodGen
* Parser, bindings, type handling etc

#### src/Longtail
* Extensions and helpers for the generated bindings. These are updated manually.

#### src/Longtail/Generated
* Longtail.cs contains all structs, enums etc
* LongtailLibrary.cs contains all the longtail functions found in longtail.h (and all other h files in the repo)

