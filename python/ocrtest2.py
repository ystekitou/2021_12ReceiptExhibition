from PIL import Image
import sys

import pyocr
import pyocr.builders
import cv2
import numpy as np

input_original_data = 'paper.jpg'
img = cv2.imread(input_original_data)
h, s, gray = cv2.split(cv2.cvtColor(img, cv2.COLOR_BGR2HSV))

size = (3, 3)
blur = cv2.GaussianBlur(gray, size, 0)

lap = cv2.Laplacian(blur, cv2.CV_8UC1)

ret2, th2 = cv2.threshold(lap, 0, 255, cv2.THRESH_BINARY+cv2.THRESH_OTSU)

kernel = np.ones((3, 20), np.uint8)
closing = cv2.morphologyEx(th2, cv2.MORPH_CLOSE, kernel)

kernel = np.ones((3, 3), np.uint8)
dilation = cv2.dilate(closing, kernel, iterations = 1)
erosion = cv2.erode(dilation, kernel, iterations = 1)

lap2 = cv2.Laplacian(erosion, cv2.CV_8UC1)

contours, hierarchy = cv2.findContours(lap2, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
min_area = img.shape[0] * img.shape[1] * 1e-4
tmp = img.copy()
if len(contours) > 0:
    for i, contour in enumerate(contours):
        rect = cv2.boundingRect(contour)
        if rect[2] < 10 or rect[3] < 10:
            continue
        area = cv2.contourArea(contour)
        if area < min_area:
            continue
        
        size_x = 28
        size_y = 28
        cv2.rectangle(tmp, (rect[0], rect[1]), (rect[0]+rect[2], rect[1]+rect[3]), (0, 255, 0), 2)
        cv2.rectangle(tmp, (rect[0], rect[1]), (rect[0]+size_x, rect[1]+size_y), (0,0,255,50), cv2.FILLED)
        h = (int)(rect[2] / size_x)
        r = 0
        for x in range(h):
            #print(x)
            if x % 2 == 0:
                r = 255
            else:
                r = 0
            cv2.rectangle(tmp, (rect[0]+size_x*x, rect[1]), (rect[0]+size_x*x+(int)(size_x), rect[1]+size_y), (0,0,r, 50), cv2.FILLED)

         

cv2.imwrite('tmp.jpg', tmp)

#cv2.imshow('color', tmp)
#cv2.waitKey(0)
cv2.destroyAllWindows()