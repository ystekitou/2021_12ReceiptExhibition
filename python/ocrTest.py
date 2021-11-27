#!/usr/bin/env python
# coding:utf-8
#
# convert.py
#
# Author:   Hiromasa Ihara (taisyo)
# Created:  2015-10-04
#

#sudo apt-get update
#sudo apt-get install python-opencv
#sudo apt-get install tesseract-ocr
#sudo apt-get install tesseract-ocr-jpn

import numpy
import os
import cv2

def transform_by4(img, points):
  """ 4点を指定してトリミングする。
  source:http://blanktar.jp/blog/2015/07/python-opencv-crop-box.html
  """

  points = sorted(points, key=lambda x:x[1])  # yが小さいもの順に並び替え。
  top = sorted(points[:2], key=lambda x:x[0])  # 前半二つは四角形の上。xで並び替えると左右も分かる。
  bottom = sorted(points[2:], key=lambda x:x[0], reverse=True)  # 後半二つは四角形の下。同じくxで並び替え。
  points = numpy.array(top + bottom, dtype='float32')  # 分離した二つを再結合。

  width = max(numpy.sqrt(((points[0][0]-points[2][0])**2)*2), numpy.sqrt(((points[1][0]-points[3][0])**2)*2))
  height = max(numpy.sqrt(((points[0][1]-points[2][1])**2)*2), numpy.sqrt(((points[1][1]-points[3][1])**2)*2))

  dst = numpy.array([
      numpy.array([0, 0]),
      numpy.array([width-1, 0]),
      numpy.array([width-1, height-1]),
      numpy.array([0, height-1]),
      ], numpy.float32)

  trans = cv2.getPerspectiveTransform(points, dst)  # 変換前の座標と変換後の座標の対応を渡すと、透視変換行列を作ってくれる。
  return cv2.warpPerspective(img, trans, (int(width), int(height)))  # 透視変換行列を使って切り抜く。

def convert(filename) :
  im = cv2.imread(filename)

#グレースケール化
  im_gray = cv2.cvtColor(im, cv2.COLOR_BGR2GRAY)
  cv2.imwrite(filename+'_gray.jpg', im_gray)
  print(filename+'_gray.jpg')

  im_blur = cv2.fastNlMeansDenoising(im_gray)
  im_th = cv2.adaptiveThreshold(im_blur, 255, cv2.ADAPTIVE_THRESH_MEAN_C, cv2.THRESH_BINARY, 15, 5)
  th_filename = "{:s}_th.jpg".format(filename)
  cv2.imwrite(th_filename, im_th)
  print(filename+'_th.jpg')

#輪郭抽出
  cnts = cv2.findContours(im_th, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)[0]  # 抽出した輪郭に近似する直線（？）を探す。
  cnts.sort(key=cv2.contourArea, reverse=True)  # 面積が大きい順に並べ替える。

  im_line = im.copy()
  warp = None
  for c in cnts[1:]:
    arclen = cv2.arcLength(c, True)
    approx = cv2.approxPolyDP(c, 0.02*arclen, True)

    if len(approx) == 4:
      cv2.drawContours(im_line, [approx], -1, (0, 0, 255), 2)
      if warp is None:
        print(approx)
        warp = approx.copy()  # 一番面積の大きな四角形をwarpに保存。
    else:
      cv2.drawContours(im_line, [approx], -1, (0, 255, 0), 2)

    for pos in approx:
      cv2.circle(im_line, tuple(pos[0]), 4, (255, 0, 0))

  cv2.imshow('im_line', im_line)
  cv2.imwrite(filename+'_line.jpg', im_line)
  print(filename+'_line.jpg')

  im_rect = transform_by4(im, warp[:,0,:])
  cv2.imwrite(filename+'_rect.jpg', im_rect)
  print(filename+'_rect.jpg')

#グレースケール化
  im_rect_gray = cv2.cvtColor(im_rect, cv2.COLOR_BGR2GRAY)
  cv2.imwrite(filename+'_rect_gray.jpg', im_rect_gray)
  print(filename+'_rect_gray.jpg')

  im_rect_blur = cv2.fastNlMeansDenoising(im_rect_gray)
  im_rect_th = cv2.adaptiveThreshold(im_rect_blur, 255, cv2.ADAPTIVE_THRESH_MEAN_C, cv2.THRESH_BINARY, 63, 5)
  rect_th_filename = "{:s}_rect_th.jpg".format(filename)
  cv2.imwrite(rect_th_filename, im_rect_th)
  print(filename+'_rect_th.jpg')

  rect_th_filename = "{:s}_rect_th.jpg".format(filename)
  os.system("tesseract {:s} out -l jpn".format(rect_th_filename))
  text = open("./out.txt").read().replace('ー', '1').replace('\\', '¥')
  print(text)

if __name__ == '__main__':
  convert("./paper.jpg")