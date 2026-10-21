using System;
using System.Threading.Tasks;
using CSharpMastery.Topics.Topic01;
using CSharpMastery.Topics.Topic02;
using CSharpMastery.Topics.Topic03;
using CSharpMastery.Topics.Topic04;
using CSharpMastery.Topics.Topic05;
using CSharpMastery.Topics.Topic06;
using CSharpMastery.Topics.Topic07;
using CSharpMastery.Topics.Topic08;
using CSharpMastery.Topics.Topic09;
using CSharpMastery.Topics.Topic10;
using CSharpMastery.Topics.Topic11;
using CSharpMastery.Topics.Topic12;
using CSharpMastery.Topics.Topic13;
using CSharpMastery.Topics.Topic14;
using CSharpMastery.Topics.Topic15;
using CSharpMastery.Topics.Topic16;
using CSharpMastery.Topics.Topic17;
using CSharpMastery.Topics.Topic18;
using CSharpMastery.Topics.Topic19;
using CSharpMastery.Topics.Topic20;
using CSharpMastery.Topics.Topic21;
using CSharpMastery.Topics.Topic22;
using CSharpMastery.Topics.Topic23;
using CSharpMastery.Topics.Topic24;
using CSharpMastery.Topics.Topic25;
using CSharpMastery.Topics.Topic26;

Console.WriteLine("===============================================================");
Console.WriteLine("        🚀 COMPLETE C# MASTERY CURRICULUM RUNNER               ");
Console.WriteLine("===============================================================");
Console.WriteLine("Phase 1: Language Foundations (01 - 07)");
Console.WriteLine("Phase 2: Object-Oriented Programming (08 - 10)");
Console.WriteLine("Phase 3: Collections & Data Structures (11 - 13)");
Console.WriteLine("Phase 4: Error Handling & File I/O (14 - 15)");
Console.WriteLine("Phase 5: Advanced Language Features (16 - 20)");
Console.WriteLine("Phase 6: .NET Ecosystem & Architecture (21 - 24)");
Console.WriteLine("Phase 7: Testing & Code Quality (25 - 26)");
Console.WriteLine("---------------------------------------------------------------");
Console.WriteLine("Enter topic number (1 to 26), or press Enter for ALL topics:");
Console.Write("Choice: ");

string? input = Console.ReadLine()?.Trim();

if (string.IsNullOrEmpty(input) || input.Equals("ALL", StringComparison.OrdinalIgnoreCase))
{
    await RunAllTopicsAsync();
}
else if (int.TryParse(input, out int topicNumber))
{
    await RunSingleTopicAsync(topicNumber);
}
else
{
    Console.WriteLine("Invalid selection. Exiting.");
}

static async Task RunSingleTopicAsync(int topic)
{
    switch (topic)
    {
        case 1: IntroDemo.Run(); break;
        case 2: FundamentalsDemo.Run(); break;
        case 3: OperatorsDemo.Run(); break;
        case 4: LoopsDemo.Run(); break;
        case 5: MethodsDemo.Run(); break;
        case 6: ArraysStringsDemo.Run(); break;
        case 7: BuiltInTypesDemo.Run(); break;
        case 8: OOPFundamentalsDemo.Run(); break;
        case 9: OOPPolymorphismDemo.Run(); break;
        case 10: AdvancedOOPDemo.Run(); break;
        case 11: CollectionsDemo.Run(); break;
        case 12: GenericsDemo.Run(); break;
        case 13: LINQDemo.Run(); break;
        case 14: ExceptionHandlingDemo.Run(); break;
        case 15: FileIODemo.Run(); break;
        case 16: DelegatesEventsDemo.Run(); break;
        case 17: ModernFeaturesDemo.Run(); break;
        case 18: await AsyncDemo.RunAsync(); break;
        case 19: ThreadingDemo.Run(); break;
        case 20: MemoryDemo.Run(); break;
        case 21: PlatformDemo.Run(); break;
        case 22: DatabaseDemo.Run(); break;
        case 23: AspNetCoreDemo.Run(); break;
        case 24: DesktopDemo.Run(); break;
        case 25: UnitTestingDemo.Run(); break;
        case 26: DebuggingDemo.Run(); break;
        default: Console.WriteLine($"Topic {topic} not recognized. Choose between 1 and 26."); break;
    }
}

static async Task RunAllTopicsAsync()
{
    IntroDemo.Run();
    FundamentalsDemo.Run();
    OperatorsDemo.Run();
    LoopsDemo.Run();
    MethodsDemo.Run();
    ArraysStringsDemo.Run();
    BuiltInTypesDemo.Run();
    OOPFundamentalsDemo.Run();
    OOPPolymorphismDemo.Run();
    AdvancedOOPDemo.Run();
    CollectionsDemo.Run();
    GenericsDemo.Run();
    LINQDemo.Run();
    ExceptionHandlingDemo.Run();
    FileIODemo.Run();
    DelegatesEventsDemo.Run();
    ModernFeaturesDemo.Run();
    await AsyncDemo.RunAsync();
    ThreadingDemo.Run();
    MemoryDemo.Run();
    PlatformDemo.Run();
    DatabaseDemo.Run();
    AspNetCoreDemo.Run();
    DesktopDemo.Run();
    UnitTestingDemo.Run();
    DebuggingDemo.Run();

    Console.WriteLine("\n===============================================================");
    Console.WriteLine(" ✅ ALL 26 TOPICS EXECUTED SUCCESSFULLY!");
    Console.WriteLine("===============================================================");
}
