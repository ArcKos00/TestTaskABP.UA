using FluentAssertions;
using Moq;
using TestWebAPI.Data.Dtos;
using TestWebAPI.Data.Models;
using TestWebAPI.Service;
using TestWebAPI.Service.Interfaces;

namespace UnitTestWebAPI.Services;

public class ServiceTest
{
    private readonly IService _service;

    private readonly Mock<IDeviceRepository> _device;
    private readonly Mock<IExperimentRepository> _experiment;
    private readonly Mock<Random> _rand;

    public ServiceTest()
    {
        _device = new Mock<IDeviceRepository>();
        _device.As<ICounter>();
        _experiment = new Mock<IExperimentRepository>();
        _rand = new Mock<Random>();

        _service = new Service(_device.Object, _experiment.Object, _rand.Object);
    }

    [Fact]
    public async Task ButtonColor_Successful()
    {
        // arrange
        var token = "test";
        var exp = "button_color";
        var value = "#FF0000";

        var expectedResult = new Response<string>
        {
            Key = exp,
            Value = value,
        };
        var testDevice = new DevicesForExperiment
        {
            Experiment = new Experiment
            {
                Name = exp,
            },
            Value = value
        };

        _device.Setup(s => s.Get(It.Is<string>(i => i.Equals(token)))).ReturnsAsync(testDevice);

        // act
        var result = await _service.ButtonColor(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task ButtonColor_Failed_Null_Case0()
    {
        // arrange
        var token = "test";
        var value = "#FF0000";
        var exp = "button_color";
        var expId = 1;
        var count = 0;
        DevicesForExperiment device = null!;
        var expected = new Response<string>
        {
            Key = exp,
            Value = value,
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);
        _device.As<ICounter>().Setup(s => s.Count(exp)).Returns(count);
        _experiment.Setup(s => s.GetExpByName(It.Is<string>(i => i.Equals(exp)))).ReturnsAsync(expId);


        // act
        var result = await _service.ButtonColor(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ButtonColor_Failed_Null_Case1()
    {
        // arrange
        var token = "test";
        var value = "#00FF00";
        var exp = "button_color";
        var expId = 1;
        var count = 1;
        DevicesForExperiment device = null!;
        var expected = new Response<string>
        {
            Key = exp,
            Value = value
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);
        _device.As<ICounter>().Setup(s => s.Count(exp)).Returns(count);
        _experiment.Setup(s => s.GetExpByName(It.Is<string>(i => i.Equals(exp)))).ReturnsAsync(expId);

        // act
        var result = await _service.ButtonColor(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ButtonColor_Failed_Null_Case2()
    {
        // arrange
        var token = "test";
        var value = "#0000FF";
        var exp = "button_color";
        var expId = 1;
        var count = 2;
        DevicesForExperiment device = null!;
        var expected = new Response<string>
        {
            Key = exp,
            Value= value
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);
        _device.As<ICounter>().Setup(s => s.Count(exp)).Returns(count);
        _experiment.Setup(s => s.GetExpByName(It.Is<string>(i => i.Equals(exp)))).ReturnsAsync(expId);

        // act
        var result = await _service.ButtonColor(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ButtonColor_Failed_Exp()
    {
        // arrange
        var token = "test";
        var exp = "price";
        var device = new DevicesForExperiment
        {
            Experiment = new Experiment { Name = exp }
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);

        // act
        var exception = await Assert.ThrowsAsync<Exception>(async () => await _service.ButtonColor(token));

        // assert
        exception.Message.Should().Be("The device is already being used in another exam");
    }

    [Fact]
    public async Task Price_Successful()
    {
        // arrange
        var token = "test";
        var exp = "price";
        var value = 5;

        var expectedResult = new Response<int>
        {
            Key = exp,
            Value = value
        };
        var testDevice = new DevicesForExperiment
        {
            Experiment = new Experiment
            {
                Name = exp,
            },
            Value = value.ToString()
        };

        _device.Setup(s => s.Get(It.Is<string>(i => i.Equals(token)))).ReturnsAsync(testDevice);

        // act
        var result = await _service.Price(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task Price_Failed_Value10()
    {
        // arrange
        var token = "test";
        var value = 10;
        var exp = "price";
        var expId = 1;
        DevicesForExperiment device = null!;
        var expected = new Response<int>
        {
            Key = exp,
            Value = value
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);
        _rand.Setup(s => s.Next(It.IsAny<int>())).Returns(25);
        _experiment.Setup(s => s.GetExpByName(It.Is<string>(i => i.Equals(exp)))).ReturnsAsync(expId);

        // act
        var result = await _service.Price(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Price_Failed_Value20()
    {
        // arrange
        var token = "test";
        var value = 20;
        var exp = "price";
        var expId = 1;
        DevicesForExperiment device = null!;
        var expected = new Response<int>
        {
            Key = exp,
            Value = value
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);
        _rand.Setup(s => s.Next(It.IsAny<int>())).Returns(80);
        _experiment.Setup(s => s.GetExpByName(It.Is<string>(i => i.Equals(exp)))).ReturnsAsync(expId);

        // act
        var result = await _service.Price(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Price_Failed_Value50()
    {
        // arrange
        var token = "test";
        var value = 50;
        var exp = "price";
        var expId = 1;
        DevicesForExperiment device = null!;
        var expected = new Response<int>
        {
            Key = exp,
            Value = value
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);
        _rand.Setup(s => s.Next(It.IsAny<int>())).Returns(88);
        _experiment.Setup(s => s.GetExpByName(It.Is<string>(i => i.Equals(exp)))).ReturnsAsync(expId);

        // act
        var result = await _service.Price(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Price_Failed_Value5()
    {
        // arrange
        var token = "test";
        var value = 5;
        var exp = "price";
        var expId = 1;
        DevicesForExperiment device = null!;
        var expected = new Response<int>
        {
            Key = exp,
            Value = value
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);
        _rand.Setup(s => s.Next(It.IsAny<int>())).Returns(95);
        _experiment.Setup(s => s.GetExpByName(It.Is<string>(i => i.Equals(exp)))).ReturnsAsync(expId);

        // act
        var result = await _service.Price(token);

        // assert
        result.Should().NotBeNull();
        result.Key.Should().Be(exp);
        result.Value.Should().Be(value);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Price_Failed_Exp()
    {
        // arrange
        var token = "test";
        var exp = "button_color";
        var device = new DevicesForExperiment
        {
            Experiment = new Experiment { Name = exp }
        };

        _device.Setup(s => s.Get(token)).ReturnsAsync(device);

        // act
        var exception = await Assert.ThrowsAsync<Exception>(async () => await _service.Price(token));

        // assert
        exception.Message.Should().Be("The device is already being used in another exam");
    }
}
