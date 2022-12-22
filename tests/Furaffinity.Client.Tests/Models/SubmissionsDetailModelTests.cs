using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Furaffinity.Client.Models;
using Xunit;

namespace Furaffinity.Client.Tests.Models;

public class SubmissionsDetailModelTests
{
    [Theory]
    [InlineData(typeof(Category))]
    [InlineData(typeof(Gender))]
    [InlineData(typeof(Rating))]
    [InlineData(typeof(Species))]
    [InlineData(typeof(SubmissionId))]
    [InlineData(typeof(Theme))]
    [InlineData(typeof(Title))]
    public void CreateTest_Should_Throw_Null_Exception(Type model)
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var constructorInfo = model.GetTypeInfo().DeclaredConstructors.First();

            try
            {
                constructorInfo.Invoke(new object?[] {" "});
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException!;
            }
        });
    }

    [Theory]
    [InlineData(typeof(Category))]
    [InlineData(typeof(Gender))]
    [InlineData(typeof(Rating))]
    [InlineData(typeof(Species))]
    [InlineData(typeof(Theme))]
    public void CreateTest_Should_Throw_Invalid_Exception(Type model)
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var constructorInfo = model.GetTypeInfo().DeclaredConstructors.First();

            try
            {
                constructorInfo.Invoke(new object?[] {"ssdfagsdfbv"});
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException!;
            }
        });
    }
}