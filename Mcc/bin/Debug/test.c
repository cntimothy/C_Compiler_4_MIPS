#include "stdio.h"
#include "conio.h"
void main(void)
{
  int x,y,z,t;
  x = 3;
  y = 8;
  z = 4;
  if (x>y)
    {t=x;x=y;y=t;} /*交换x,y的值*/
  if(x>z)
    {t=z;z=x;x=t;} /*交换x,z的值*/
  if(y>z)
    {t=y;y=z;z=t;} /*交换z,y的值*/
}