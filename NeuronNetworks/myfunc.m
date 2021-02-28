function [xmax] = myfunc(x,y,z) 

plot(x,y,x,z);
ylim([0 1])
grid on;

xmax=0