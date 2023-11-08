namespace TestProjectCafeExtensions
{
    public class UnitTest1
    {
        [Fact]
        public void GenerateDescription_ValidInput_ReturnsTrimmedDescription()
        {
            // Arrange
            string title = "QIRD ������������ ����� ��������� ���������� ��� �������� ���������";
            string body = "�� ���� �������� �� ������������ ������� ��� ���� ����� �������� - �������� Qird ������������ ���������� ����� ��������� ����������, ������������� ��� ���, ����� ������� ���� ����������� ������ ��� ����� ���������� � ���������. ��� ���������� - ��������� ����� ���������� ������ ��� ���, ����� ������������ ��� ������ ���� � ������� ����������� �����. �� ������� ��� ����������� ������� � ��������������, ����� ������������� ��� ���� �����������. ��� �� ������� � ����� ����� ��������� ����������: ������ �� ����� � ���� �������: ������ ������ � ����� ����� �����, ��� �����-����. �� ������ ������� ������� ��� ��� ����� � �����������, � ���������� ����� ����������� ���������. �������� ����������� ��������: ������ �� ����� ����� ��� ��������� � �������, ����� ������ ���������� ��������. ��� ������ ������ �������� � ����� ��������� ����������. �������� � ������������� ������: ���� ����������� ������� ������ ������ ��� �����. �� ������ ������������� ���� ������ � ���������������� ������ �� �������. ����� � ��������: ���� � ��� ���� ������� ��� ����� ������, �� ������ ����� ��������� � �������� ����� ����������. �� ����� �������� ��������������� ������������ ��������������� ���������� ��� ����� ������-���������, ��� ���� �� ����������� ��������� ���� ���������� ����. �� �������� ���� �������� ��� ���� ������������ ������������ � ����� ����� ��������� �����������. ��� �������� ��� ���������� � ��������� ���������� ����� ������. ��������� � ����� ������� �������� � �������� � ����������� ������ � Qird!";
            int maxDescriptionLength = 100;

            // Act
            string description = CafeExtensions.Services.SEOGenerator.GenerateDescription(title, body, maxDescriptionLength);

            // Assert
            Assert.True(description.Length <= maxDescriptionLength);
        }

        [Fact]
        public void GenerateDescription_LongInput_TruncatesDescription()
        {
            // Arrange
            string title = "Long Page Title with Many Words";
            string body = "This is a very long text with many words to exceed the maximum description length.";
            int maxDescriptionLength = 20;

            // Act
            string description = CafeExtensions.Services.SEOGenerator.GenerateDescription(title, body, maxDescriptionLength);

            // Assert
            Assert.True(description.Length <= maxDescriptionLength);
        }
    }
}