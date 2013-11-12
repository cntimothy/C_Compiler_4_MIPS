#include "stdio.h"
#include "conio.h"
void main(void)
{
  int x,y,z,t;
  x = 3;
  y = 8;
  z = 4;
  if (x>y)
    {t=x;x=y;y=t;} /*exchange x and y*/
  if(x>z)
    {t=z;z=x;x=t;} /*exchange x and z*/
  if(y>z)
    {t=y;y=z;z=t;} /*exchange y and z*/
}