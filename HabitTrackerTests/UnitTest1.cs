using habit_tracker;

namespace HabitTrackerTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InputtingValidNumber_WhenGetNumberInputIsCalled_ReturnsTheNumber()
        {
            //Arrange
            string input = "5";

            //Act
            var result = Tracker.GetNumberInput(input);
            //Assert
            Assert.Equals(5, result);
        }
    }
}
