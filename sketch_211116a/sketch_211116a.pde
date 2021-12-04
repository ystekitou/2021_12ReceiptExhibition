import java.util.ArrayList; //<>//
import java.util.Collections;
PImage img, img2, img3;

int dotSize = 28;
float [] r = new float[dotSize*dotSize];
float [] rN = new float[dotSize*dotSize];
Dot d1, d2;
int stage = 0;

PImage[] imgs = new PImage[3];

void setup_2(){
  size(64,64);
  PImage p = loadImage("zombi_01.jpg");
  p.resize(64,64);
  image(p,0,0);
  saveFrame("64Conv.png");
  
}

void setup() {
  
  //size(1280, 720);
  fullScreen();
  frameRate(30);
  //img = loadImage("64Average.bmp");
  img = loadImage("zombi64.png");
  img2 = loadImage("dot.bmp");
  img3 = loadImage("korokke.gif");
  imgs = new PImage[]{img, img2};
  d1 = new Dot(img, img2);
  //  d2 = new Dot(img2);
}

void draw() {
  background(255);
  scale(0.4);
  d1.paint();
  if (d1.isMoveComp) {

    d1 = new Dot(d1.img2, imgs[(int)random(0, imgs.length)]);
  }
}




void mousePressed() {
  saveFrame("image.png");
}
