using ChainStoreApiBusiness;

namespace Tests.ChainStore;

public class UnitTest
{
    //validate password if its longer than 6

    [Fact]
    public void validatePassword()
    {
        // arrange
        string password = "greater6789";
        bool expected = true;

        // act
        bool actual = ChainstoreUtility.validatePassword(password);

        //assert
        Assert.Equal(expected, actual);
    }

    //validate if email contains @

    [Fact]
    public void validateEmail()
    {
        // arrange
        string emailaddress = "obi@gmail.com";
        bool expected = true;

        // act
        bool actual = ChainstoreUtility.verifyEmail(emailaddress);

        //assert
        Assert.Equal(expected, actual);

    }


    //validate if quantity odered is an int
    [Fact]
    public void validatequant()
    {
        // arrange
        string quantity = "6";
        bool expected = false;

        // act
        bool actual = ChainstoreUtility.verifyEmail(quantity);

        //assert
        Assert.Equal(expected, actual);

    }








}

