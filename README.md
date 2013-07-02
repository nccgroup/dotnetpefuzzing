dotnetpefuzzing
===============

NCC Code Navi the Text Viewer and Searcher for Code Reviewers

Released as open source by NCC Group Plc - http://www.nccgroup.com/

Developed by Ollie Whitehouse, ollie dot whitehouse at nccgroup dot com

http://www.github.com/nccgroup/dotnetpefuzzing

Released under AGPL see LICENSE for more information

Introduction
-----------
Tiny .NET PE Fuzzing Harness - Proof of Concept

Components
-----------
This code was released to support an NCC blog post. There are three basic components:
* Win.DotNetAssemblyLoad - the loader harness which watches a directory a tries to load any modules
* Win.Module - a simple managed .NET module (i.e. DLL)
* Win.NULLFuzzer - a simple fuzz which just walks through and set each byte of the input file to null

Running
-----------
It would be run something like this:
* Compile the module
* Start the harness watching a directory
* Attach WinDbg / debugger of choice to the harness to see any interesting yet handled exceptions in unmanaged code
* Fuzz and produce DLL test cases in the directory the harness is monitoring

Basic Example
-----------
* Win.DotNetAssemblyLoad C:\!Research\Fuzzing\DotNetPE\TestCases
* now attached WinDbg to the process
* Win.NULLFuzzer.exe C:\!Research\DotNetPE\Win.Module.dll C:\!Research\Fuzzing\DotNetPE\TestCases

WARNING
-----------
This simple example on how to approach the problem did not find any issues in the 4.x runtime :) 