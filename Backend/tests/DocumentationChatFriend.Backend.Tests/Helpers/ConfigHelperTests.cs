using DocumentationChatFriend.Backend.Api.Helpers;

namespace DocumentationChatFriend.Backend.Tests.Helpers;

public class ConfigHelperTests
{
    public static IEnumerable<object?[]> TestData =>
        new List<object?[]>
        {
            new object[] { "hello", false },
            new object[] { "", true },
            new object?[] { null, true },
            new object[] { new TestClass(), false },
            new object?[] { 42, false },
            new object?[] { (int?)null, true },
            new object?[] { 4.6, false },
            new object?[] { (double?)null, true },
            new object?[] { (TestStruct?)new TestStruct(), false },
            new object?[] { (TestStruct?)null, true }
        };

    [Theory]
    [MemberData(nameof(TestData))]
    public void MustBeSet_ReferenceTypes_ThrowsWhenExpected(object input, bool shouldThrow)
    {

        if (shouldThrow)
        {
            Assert.Throws<InvalidOperationException>(() => ConfigHelper.MustBeSet(input, "test"));
        }

    }

}

file class TestClass
{

}

file struct TestStruct
{

}