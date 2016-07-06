instrreset
clear all
close all
clc

% UDPComIn=udp('127.0.0.1','LocalPort',1208, 'inputbuffersize', 1024);
UDPComIn=udp('127.0.0.1','LocalPort',1222)
% UDPComIn=udp('192.168.10.190','LocalPort',11111);
% set(UDPComIn,'DatagramTerminateMode','off')
% csvdata = char() 
fopen(UDPComIn);


interval = 8000;  %Number of Samples (Default = 30FPS)
data = [];
% signal = [];
y = NaN(300,1);


for t = 1:interval
    csvdata=fscanf(UDPComIn);
    scandata = textscan(csvdata,'%s %f %s %s %s %f %s %f %s %f %s %f %s %s' , 'Delimiter',',;');
    data = scandata{6};
    for s = 1:size(data,1)
        y = circshift(y, -1);
        y(end) = data(s);
        plot(y,'r','linewidth',2)
        xlabel('Frames per Second');
        ylabel('Heart Rate (BPM)')
        title('HR from Android Wearable')
        axis([0 inf 0 180])
        grid
        drawnow
    end
% toc
end
fclose(UDPComIn);
display(data)