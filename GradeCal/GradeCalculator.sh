
#!

#single intry

echo "enter the raw score here"

read score

if [ $score -lt 0 ]
then
    echo " Negative numbers not accepted"
    break
fi

if [ $score -ge 90 ] 
then
    grade="A"
elif [ $score -ge 80 ]
then
    grade="B"
elif [ $score -ge 70 ]
then 
    grade="C"
elif [ $score -ge 60 ]
then
    grade="D"
else
    grade="F"
fi

echo "Your grade is $grade"


