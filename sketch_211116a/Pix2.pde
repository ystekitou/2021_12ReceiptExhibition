class Pix2 extends Pix_Base {
  float xN = random(1);
  float yN = random(1);
  float rN = random(1);
  Pix2(int x, int y, color cc, int dotSize) {
    this.baseX= x * 32;
    this.baseY = y * 32;
    r = dotSize;
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
      ////r = random(32);
      //xN += 0.01;
      //yN += 0.01;
      //rN += random(0.01, 0.1);
    } else if (mode == 2) {
      noStroke();
      color cc = lerpColor(c, c2, lerpCount);
      fill(red(cc), green(cc), blue(cc), alpha(cc));
      rect(x, y, r, r);
      x += vx;
      y += vy;
      vy += 0.5;
    }


    if (mousePressed) {
      isMove = true;
      mode = 1;
    }
  }
}
