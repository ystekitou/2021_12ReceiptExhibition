public abstract class Pix_Base {
  public float x, y, vx, vy, baseX, baseY, r;
  public float dx, dy;
  public color c, c2;

  public int mode;
  public boolean isMove = false;

  Pix_Base() {
  }

  Pix_Base(int x, int y, color cc, int dotSize) {
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


  abstract void setV(float dx, float dy, color dc); //目的座標を入力、速度決定
  abstract void paint(); //速度変更
}
