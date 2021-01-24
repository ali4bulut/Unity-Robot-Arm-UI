#include <Arduino.h>
#include <AccelStepper.h>

#define bufferSize 48

AccelStepper eksen1(1, 2, 3);
AccelStepper eksen2(1, 4, 5);
AccelStepper eksen3(1, 6, 7);
AccelStepper eksen4(1, 8, 9);
AccelStepper eksen5(1, 10, 11);
AccelStepper eksen6(1, 12, 13);

const int switches[] = {30, 32, 34, 36, 38, 40};

float acilar[6];
int i = 0;

void Homing(AccelStepper *stepMotor, int homeSwitchNumber)
{
  while (digitalRead(switches[homeSwitchNumber - 1]))
  {
    stepMotor->move(32);
    stepMotor->run();
    delay(5);
  }

  stepMotor->setCurrentPosition(0);

  while (!digitalRead(switches[homeSwitchNumber - 1]))
  {
    stepMotor->move(-32);
    stepMotor->run();
    delay(5);
  }
  stepMotor->setCurrentPosition(0);
}

void Run(AccelStepper *stepMotor, unsigned int angle)
{
  while (stepMotor->distanceToGo() == 0)
  {
    stepMotor->moveTo(angle);
    stepMotor->run();
  }
}

void setup()
{
  Serial.begin(2000000);
  Serial.setTimeout(5);

  eksen1.setMaxSpeed(12800);
  eksen1.setAcceleration(9600);

  eksen2.setMaxSpeed(12800);
  eksen2.setAcceleration(9600);

  eksen3.setMaxSpeed(12800);
  eksen3.setAcceleration(9600);

  eksen4.setMaxSpeed(12800);
  eksen4.setAcceleration(9600);

  eksen5.setMaxSpeed(12800);
  eksen5.setAcceleration(9600);

  eksen6.setMaxSpeed(12800);
  eksen6.setAcceleration(9600);

  pinMode(13, OUTPUT);

  // Homing(&eksen1, 1);
  // Homing(&eksen2, 2);
  // Homing(&eksen3, 3);
  // Homing(&eksen4, 4);
  // Homing(&eksen5, 5);
  // Homing(&eksen6, 6);

  //delay(1000);

  // Bütün eksenler hesaplanan sıfır pozisyonuna gidecek TODO:
  // Run(&eksen1, 170);
  // Run(&eksen2, 145);
  // Run(&eksen3, 70);
  // Run(&eksen4, 180);
  // Run(&eksen5, 90);
  // Run(&eksen6, 180);
}

void loop()
{
  // If at the end of travel go to the other end
  // if (eksen1.distanceToGo() == 0)
  //   eksen1.moveTo(-eksen1.currentPosition());

  // eksen1.run();
  if (Serial.available() > 0)
  {
    char input[bufferSize + 1];
    byte size = Serial.readBytes(input, bufferSize);
    input[size] = 0;

    char *command = strtok(input, "&");
    while (command != NULL)
    {
      float split = atof(command);
      acilar[i] = split ;/// 0.05625f; // 1/32 step olduğu için:
      i++;

      command = strtok(NULL, "&");
    }
    i = 0;
    for (int j = 0; j < 6; j++)
    {
      Serial.print(acilar[j]);
      Serial.print(" / ");
    }
    Serial.print("\n");

    // eksen1.moveTo(acilar[0]);
    // eksen1.run();
    // eksen2.moveTo(acilar[1]);
    // eksen2.run();
    // eksen3.moveTo(acilar[2]);
    // eksen3.run();
    // eksen4.moveTo(acilar[3]);
    // eksen4.run();
    // eksen5.moveTo(acilar[4]);
    // eksen5.run();
    // eksen6.moveTo(acilar[5]);
    // eksen6.run();
  }

  delay(5);
}
