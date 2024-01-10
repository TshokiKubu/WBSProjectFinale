using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using WSB.API.Services;
using Xunit;

namespace WSB.TestAPI
{
    public class RedisCacheServiceTests
    {
        [Fact]
        public async Task GetOrSetAsync_Should_Return_Cached_Value()
        {
            // Arrange
            var mockCache = new Mock<IDistributedCache>();
            var redisCacheService = new RedisCacheService(mockCache.Object);

            var key = "TestKey";
            var cachedValue = "CachedValue";

           // mockCache.Setup(x => x.GetStringAsync(It.IsAny<string>()))
            //         .ReturnsAsync(JsonConvert.SerializeObject(cachedValue));

         ////   mockCache.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<int>()))
         //.ReturnsAsync(JsonConvert.SerializeObject(cachedValue));

         //   mockCache.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<OptionalType>()))
         //.ReturnsAsync(JsonConvert.SerializeObject(cachedValue));

         //   mockCache.Setup(x => x.GetStringAsync(It.IsAny<int>()))
         //   .ReturnsAsync(JsonConvert.SerializeObject(cachedValue));


            // Act
            var result = await redisCacheService.GetOrSetAsync(key, () => Task.FromResult("NotCachedValue"), new DistributedCacheEntryOptions());

            // Assert
            Assert.Equal(cachedValue, result);
        }

        [Fact]
        public async Task GetOrSetAsync_Should_Call_GetItemCallback_When_Cache_Miss()
        {
            // Arrange
            var mockCache = new Mock<IDistributedCache>();
            var redisCacheService = new RedisCacheService(mockCache.Object);

            var key = "TestKey";
            var notCachedValue = "NotCachedValue";

          //  mockCache.Setup(x => x.GetStringAsync(It.IsAny<string>()))
          //           .ReturnsAsync((string)null);

            // Act
            var result = await redisCacheService.GetOrSetAsync(key, () => Task.FromResult(notCachedValue), new DistributedCacheEntryOptions());

            // Assert
            Assert.Equal(notCachedValue, result);
        }

        [Fact]
        public async Task GetOrSetAsync_Should_Set_Cache_When_Cache_Miss()
        {
            // Arrange
            var mockCache = new Mock<IDistributedCache>();
            var redisCacheService = new RedisCacheService(mockCache.Object);

            var key = "TestKey";
            var notCachedValue = "NotCachedValue";

           // mockCache.Setup(x => x.GetStringAsync(It.IsAny<string>()))
          //           .ReturnsAsync((string)null);

            // Act
            var result = await redisCacheService.GetOrSetAsync(key, () => Task.FromResult(notCachedValue), new DistributedCacheEntryOptions());

            // Assert
       //     mockCache.Verify(x => x.SetStringAsync(key, JsonConvert.SerializeObject(notCachedValue), It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }

     
    }
}