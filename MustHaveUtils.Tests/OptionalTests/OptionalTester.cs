using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MustHaveUtils.Tests.OptionalTests
{
    public class OptionalTester
    {
        [Fact]
        public void EmptyOptional_HasValue_False()
        {
            var optional = Optional.Empty<string>();

            Assert.False(optional.HasValue);
        }

        [Fact]
        public void EmptyOptional_GetValue_Exception()
        {
            var optional = Optional.Empty<string>();

            Assert.Throws<InvalidOperationException>(() => optional.GetValue);
        }

        [Fact]
        public void EmptyOptional_OrElse_NewValue()
        {
            var optional = Optional.Empty<string>();

            var result = optional.OrElse("testElse");

            Assert.Equal("testElse", result);
        }

        [Fact]
        public void EmptyOptional_OrElseGet_OnceInvoked()
        {
            var mockAction = new Mock<Func<string>>();
            mockAction.Setup(p => p.Invoke())
                .Returns(It.IsAny<string>());

            var optional = Optional.Empty<string>();

            var result = optional.OrElseGet(mockAction.Object);

            mockAction.Verify(p => p.Invoke(), Times.Once);
        }

        [Fact]
        public void EmptyOptional_OrElseGet_ReturnValue()
        {
            var optional = Optional.Empty<string>();

            var result = optional.OrElseGet(() => "testOrElseGet");

            Assert.Equal("testOrElseGet", result);
        }

        [Fact]
        public void EmptyOptional_IfPresent_NeverInvoked()
        {
            var mockAction = new Mock<Action<string>>();
            mockAction.Setup(p => p.Invoke(It.IsAny<string>()));

            var optional = Optional.Empty<string>();
            optional.IfPresent(mockAction.Object);

            mockAction.Verify(p => p.Invoke(It.IsAny<string>()), Times.Never);
        }
    }
}
