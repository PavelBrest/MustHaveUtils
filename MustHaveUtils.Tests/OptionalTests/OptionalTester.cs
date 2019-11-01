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
        public void EmptyOptional_GetValue_InvalidOperationException()
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

        [Fact]
        public void EmptyOptional_IfPresent_ArgumentNullException()
        {
            var optional = Optional.Of(value: "test");

            Assert.Throws<ArgumentNullException>(() => optional.IfPresent(null));
        }

        [Fact]
        public void EmptyOptional_Where_NeverInvoked()
        {
            var mockAction = new Mock<Func<string, bool>>();
            mockAction.Setup(p => p.Invoke(It.IsAny<string>()))
                .Returns(false);

            var optional = Optional.Empty<string>();
            optional.Where(mockAction.Object);

            mockAction.Verify(p => p.Invoke(It.IsAny<string>()), Times.Never);
            Assert.False(optional.HasValue);
        }

        [Fact]
        public void EmptyOptional_Where_ArgumentNullException()
        {
            var optional = Optional.Empty<string>();

            Assert.Throws<ArgumentNullException>(() => optional.Where(null));
        }

        [Fact]
        public void EmptyOptional_Map_NeverInvoked()
        {
            var mockAction = new Mock<Func<string, int>>();
            mockAction.Setup(p => p.Invoke(It.IsAny<string>()));

            var optional = Optional.Empty<string>()
                .Map(mockAction.Object);

            mockAction.Verify(p => p.Invoke(It.IsAny<string>()), Times.Never);
            Assert.False(optional.HasValue);
        }

        [Fact]
        public void EmptyOptional_Map_ArgumentNullException()
        {
            var optional = Optional.Empty<string>();

            Assert.Throws<ArgumentNullException>(() => optional.Map<int>(null));
        }

        [Fact]
        public void OfOptional_HasValue_True()
        {
            int value = 10;

            var optional = Optional.Of(value);

            Assert.True(optional.HasValue);
        }

        [Fact]
        public void OfOptional_GetValue()
        {
            int value = 10;

            var optional = Optional.Of(value);

            Assert.Equal(value, optional.GetValue);
        }

        [Fact]
        public void OfOptional_IfPresent_InvokedOnce()
        {
            int value = 10;
            var mockAction = new Mock<Action<int>>();
            mockAction.Setup(p => p.Invoke(It.IsAny<int>()));

            var optional = Optional.Of(value);
            optional.IfPresent(mockAction.Object);

            mockAction.Verify(p => p.Invoke(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void OfOptional_OrElse_OldValue()
        {
            int value = 10;
            int elseValue = -1;

            var result = Optional.Of(value)
                .OrElse(elseValue);

            Assert.Equal(value, result);
            Assert.NotEqual(elseValue, result);
        }

        [Fact]
        public void OfOptioanl_OrElseGet_NeverInvoked()
        {
            int value = 10;
            var mockAction = new Mock<Func<int>>();
            mockAction.Setup(p => p.Invoke())
                .Returns(-1);

            var optional = Optional.Of(value);

            var result = optional.OrElseGet(mockAction.Object);

            mockAction.Verify(p => p.Invoke(), Times.Never);
            Assert.Equal(result, value);
        }

        [Fact]
        public void OfOptional_Where_ReturnEmpty()
        {
            int value = 10;

            var result = Optional.Of(value).Where(p => p > 15);

            Assert.False(result.HasValue);
        }

        [Fact]
        public void OfOptional_Where_ReturnValue()
        {
            int value = 10;

            var result = Optional.Of(value).Where(p => p < 15);

            Assert.True(result.HasValue);
            Assert.Equal(value, result.GetValue);
        }

        [Fact]
        public void OfOptional_Map_InvokedOnce()
        {
            int value = 10;
            string mappedValue = "10";

            var optional = Optional.Of(value)
                .Map(p => p.ToString());

            Assert.True(optional.HasValue);
            Assert.Equal(mappedValue, optional.GetValue);
        }
    }
}
