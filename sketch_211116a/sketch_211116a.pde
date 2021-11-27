import java.util.ArrayList; //<>//
import java.util.Collections;
PImage img, img2, img3;

float [] r = new float[16*16];
float [] rN = new float[16*16];
Dot d1, d2;
int stage = 0;

PImage[] imgs = new PImage[3];

void setup() {
  size(1280, 720);
  img = loadImage("food_cake.gif");
  img2 = loadImage("dot2.png");
  img3 = loadImage("korokke.gif");
  imgs = new PImage[]{img, img2, img3};
  d1 = new Dot(img, img2);
  //  d2 = new Dot(img2);
}

void draw() {
  background(255);

  d1.paint();
  if(d1.isMoveComp){
    
    d1 = new Dot(d1.img2, imgs[(int)random(0,3)]);
  }
  
}

class Dot {
  PImage img, img2;
  float x, y;
  Pix [] pix = new Pix[16*16];
  Pix [] pix2 = new Pix[16*16];

  ArrayList<Pix> pixTmp = new ArrayList<Pix>();
  ArrayList<Pix> pix2Tmp = new ArrayList<Pix>();

  boolean isMoveComp = false;

  Dot(PImage pimg, PImage img2) {
    int count = 0;
    img = pimg;
    this.img2 = img2;
    for (int y = 0; y < img.height; y++) {
      for (int x = 0; x < img.width; x++) {
        color c = img.get(x, y);
        pixTmp.add(new Pix(x, y, c));
        c = img2.get(x, y);
        pix2Tmp.add(new Pix(x, y, c));
      }
    }

    Collections.shuffle(pix2Tmp);
    for (int i = 0; i < pixTmp.size(); i++) {
      pixTmp.get(i).setV(pix2Tmp.get(i).x, pix2Tmp.get(i).y, pix2Tmp.get(i).c);
    }

    pix = pixTmp.toArray(new Pix[pixTmp.size()]);
    pix2 = pix2Tmp.toArray(new Pix[pix2Tmp.size()]);
  }

  void paint() {
    isMoveComp = true;
    for (Pix p : pix) {
      p.paint();
      if(p.mode != 2){
        isMoveComp = false;
      }
    }
  }

  void setMove(boolean m) {
    for (Pix p : pix) {
    }
  }
}

class Pix {
  public float x, y, vx, vy, baseX, baseY;
  float r, vR;
  float xN=random(1), yN=random(1), rN = random(1);
  color c, c2;

  float dx, dy;

  public boolean isMove = false;

  Pix(int x, int y, color cc) {
    this.baseX= x * 32;
    this.baseY = y * 32;
    r = 32;
    c = cc;
    //fill(red(c),green(c),blue(c),alpha(c));
    //ellipse(baseX, baseY, 32,32);
    //println(alpha(c));
    this.x = baseX;
    this.y = baseY;
  }

  public void setV(float dx, float dy, color dc) {
    vx = (dx-baseX) / 100;
    vy = (dy-baseY) / 100;
    this.dx = dx;
    this.dy = dy;
    this.c2 = dc;
  }
  int mode = 0;
  float lerpCount  =0;
  void paint() {
    if (mode == 1) {
      noStroke();
      color cc = lerpColor(c, c2, lerpCount += 0.01);
      fill(red(cc), green(cc), blue(cc), alpha(cc));
      //fill(0);
      rect(x, y, r, r);
      x += vx;
      y += vy;
      //x = baseX + noise(xN) * 10;
      //y = baseY + noise(yN) * 10;
      //r = noise(rN) * 32;
      //xN += 0.01;
      //yN += 0.01;
      //rN += 0.01;
      if (dist(x, y, dx, dy) < 3) {
        vx = 0;
        vy = 0;
        x = dx;
        y = dy;
        baseX = x;
        baseY = y;
        lerpCount = 1;
        isMove = false;

        vx = random(-3, 3);
        vy = -1;
        mode = 2;
      }
    } else if (mode==0) {
      noStroke();
      color cc = lerpColor(c, c2, lerpCount);
      fill(red(cc), green(cc), blue(cc), alpha(cc));
      rect(x, y, r, r);
      //x = baseX + noise(xN) * 10;
      //y = baseY + noise(yN) * 10;
      //r = noise(rN) * 32;
      //xN += 0.01;
      //yN += 0.01;
      //rN += 0.01;
    } else if (mode == 2) {
      noStroke();
      color cc = lerpColor(c, c2, lerpCount);
      fill(red(cc), green(cc), blue(cc), alpha(cc));
      rect(x, y, r, r);
      //x += vx;
      //y += vy;
      //vy += 0.5;
    }


    if (mousePressed) {
      isMove = true;
      mode = 1;
    }
  }
}

void mousePressed(){
  saveFrame("image.png");  
}
