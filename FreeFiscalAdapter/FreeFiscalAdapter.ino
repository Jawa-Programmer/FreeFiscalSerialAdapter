#include <Wire.h>

#define ADDR          0x02

#define ANS_BTS       0x01
#define ANS_STR       0x02

#define CMD_ABOUT     0x01
#define CMD_REQUEST   0x02
#define CMD_AUTORIZE  0x03

const String ABOUT1 = "FreeFiscal software disigned by (c)JAWA inc.";
const String ABOUT2 = "https://vk.com/jawa_prog132";
const String VERSION = "Version 0.1";

uint16_t Crc16(uint8_t *pcBlock, unsigned short len)
{
  uint16_t crc = 0xFFFF;
  uint8_t i;

  while (len--)
  {
    crc ^= *pcBlock++ << 8;

    for (i = 0; i < 8; i++)
      crc = crc & 0x8000 ? (crc << 1) ^ 0x1021 : crc << 1;
  }
  return crc;
}

void sendCommand(uint8_t cmd, uint16_t argc = 0, uint8_t * args = nullptr)
{
  uint8_t *cmds = new uint8_t[3 + argc];
  uint16_t len = argc + 1;
  cmds[0] = len & 0xff;
  cmds[1] = (len >> 8) & 0xff;
  cmds[2] = cmd;
  for (int i = 0; i < argc; i++)
    cmds[i + 3] = args[i];
  uint16_t cd = Crc16(cmds, 3 + argc);
  Wire.beginTransmission(ADDR);
  Wire.write(0x04);
  for (int i = 0; i < argc + 3; i++)
    Wire.write(cmds[i]);
  Wire.write((uint8_t)(cd & 0x00ff));
  Wire.write((uint8_t)((cd & 0xff00) >> 8));
  Wire.endTransmission();
  delete[] cmds;
}
uint8_t readMessege(uint16_t *length, uint8_t **data)
{
  uint8_t PackInfo[4];
  *length = -1;
  // delay(100);
  Wire.requestFrom(ADDR, 4);
  while (Wire.available() < 4) {
    delay(10);
    Wire.requestFrom(ADDR, 4);
  }
  int i = 0;
  while (Wire.available())
  {
    PackInfo[i] = Wire.read();
    i++;
    if (i >= 4)break;
  }
  uint16_t len = (PackInfo[1] & 0x00ff)  | (((uint16_t)PackInfo[2] << 8) & 0xff00);
  uint8_t ans_code = PackInfo[3];
  uint8_t *Pack = new uint8_t[5 + len];
  Pack[0] = PackInfo[0];
  Pack[1] = PackInfo[1];
  Pack[2] = PackInfo[2];
  Pack[3] = PackInfo[3];
  Wire.requestFrom(ADDR, len + 1);
  while (Wire.available() < len + 1) {
    delay(10);
    Wire.requestFrom(ADDR, len + 1);
  }
  i = 4;
  while (Wire.available())
  {
    Pack[i] = Wire.read();
    i++;
    if (i >= len + 5)break;
  }
  *length = len + 5;
  *data = Pack;
  return ans_code;
}
const uint8_t auth[4] = {0xff, 0x00, 0xf0, 0x0f};
void setup() {
  Wire.begin();
  Serial.begin(19200);
  while (!Serial);
}
void loop() {
  if (Serial.available() > 0) {  //если есть доступные данные
    uint8_t  incomingByte = Serial.read();
    switch (incomingByte)
    {
      case CMD_AUTORIZE:
        Serial.write(auth, 4);
        break;
      case CMD_ABOUT:
        Serial.write(ANS_STR);
        Serial.println(ABOUT1);
        Serial.write(ANS_STR);
        Serial.println(ABOUT2);
        Serial.write(ANS_STR);
        Serial.println(VERSION);
        break;
      case CMD_REQUEST:
        while (!Serial.available())delay(0);
        uint8_t cmd = Serial.read();
        //Serial.write(ANS_STR);
        //Serial.println("cmd: 0x" + String(cmd, HEX));
        while (!Serial.available())delay(0);
        uint8_t argc = Serial.read();
        uint8_t *args = nullptr;
        if (argc > 0)
          args = new uint8_t[argc];
        for (int i = 0; i < argc; i++) {
          while (!Serial.available())delay(0);
          args[i] = Serial.read();
        }
        sendCommand(cmd, argc, args);
        //sendCommand(0x33, 0, nullptr);
        if (argc > 0)
          delete[] args;
        uint8_t* data = nullptr;
        uint16_t len = -1;
        uint8_t status = readMessege(&len, &data);
        if (status != 0)
        {
          Serial.write(ANS_STR);
          Serial.println("WARNING: non zero status");
        }
        if (len > 0) {
          Serial.write(ANS_BTS);
          Serial.write((uint8_t)len);
          Serial.write(data, len);
        } else
        {
          Serial.write(ANS_STR);
          Serial.println("Request finished with error");
        }
        if (data != nullptr)
          delete[] data;
        break;
    }
  }
}
