﻿// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using OData2Linq.Benchmark;

Console.WriteLine("Start benchmarking");

BenchmarkRunner.Run<InitQuery>();

Console.WriteLine("End benchmarking");