class Dot {
  PImage img, img2;
  float x, y;
  Pix_Base [] pix = new Pix_Base[dotSize*dotSize];
  Pix_Base [] pix2 = new Pix_Base[dotSize*dotSize];

  ArrayList<Pix_Base> pixTmp = new ArrayList<Pix_Base>();
  ArrayList<Pix_Base> pix2Tmp = new ArrayList<Pix_Base>();

  boolean isMoveComp = false;

  Dot(PImage pimg, PImage img2) {
    int count = 0;
    img = pimg;
    this.img2 = img2;
    for (int y = 0; y < img.height; y++) {
      for (int x = 0; x < img.width; x++) {
        color c = img.get(x, y);
        pixTmp.add(new Pix2(x, y, c, dotSize));
        c = img2.get(x, y);
        pix2Tmp.add(new Pix(x, y, c, dotSize));
        println("aaaa");
      }
    }

    Collections.shuffle(pix2Tmp);
    for (int i = 0; i < pixTmp.size(); i++) {
      //println(pixTmp.get(i).x, pix2Tmp.get(i).y, pix2Tmp.get(i).c);
      pixTmp.get(i).setV(pix2Tmp.get(i).x, pix2Tmp.get(i).y, pix2Tmp.get(i).c);
    }

    pix = pixTmp.toArray(new Pix_Base[pixTmp.size()]);
    pix2 = pix2Tmp.toArray(new Pix_Base[pix2Tmp.size()]);
  }

  void paint() {
    isMoveComp = true;
    for (Pix_Base p : pix) {
      p.paint();
      if (p.mode != 2) {
        isMoveComp = false;
      }
    }
  }

  void setMove(boolean m) {
  }
}
