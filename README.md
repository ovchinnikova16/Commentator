# .NET IL Code Generator Commentator

Commentator is a tool that allows to improve readability of C# source code generating functions that use [GrEmit][1] by adding comments with the stack state after each generated instruction.

Commentator works with C# code written for .NET 4.5. Since stack information is generated in runtime, Commentator works by running tests (currently only NUnit 3 tests) that invoke code generating functions. Commentator currently only works with MSBuild as a build system.

## Example Usage

For example, to generate comments for the code generating function, located in project `Commentator.Example` in this repository, you have to run the following command:

```
Commentator.exe \
	--gen-projects "Commentator\Commentator.Example" \
	--test-projects "Commentator\Commentator.Example.Tests" \
	--msbuild-path "<your-path-to-msbuild>\MSBuild.exe"
```

[1]: https://github.com/skbkontur/gremit