import json
import numpy as np
from colormath.color_objects import LabColor, sRGBColor
from colormath.color_conversions import convert_color
import math
import sys

# set diff and step according to your needs.


class ColorMapper:
    def __init__(self):
        self.illuminant = 'd50'
        self.starting_point = LabColor(75, 0, 0, illuminant=self.illuminant)
        self.JND = 2.5
        self.range = (1.1, 2.2, 0.1)
        self.spacing = "log"
        self.dictionary = {}
        self.values = []
        self.diff = 0.0001
        self.step = 0.005

    def setup_dictionary(self):
        for i in np.arange(self.range[0], self.range[1], self.range[2]):
            val = round(pow(10, i), 2)
            self.values.append(val)
            self.dictionary[val] = []

    def calculate_round_error(self, val, rounded):
        if abs(val-rounded) < self.diff:
            return True
        else:
            return False

    def color_distance(self, color1, color2):
        val = math.sqrt(pow(color1.lab_l-color2.lab_l, 2)+pow(color1.lab_a-color2.lab_a, 2)+pow(color1.lab_b-color2.lab_b, 2))
        return val

    def check_rgb_clamp(self, rgbcolor):
        """
        :param rgbcolor: sRGBColor
        :return:
        """
        if rgbcolor.rgb_r != rgbcolor.clamped_rgb_r:
            return False
        if rgbcolor.rgb_g != rgbcolor.clamped_rgb_g:
            return False
        if rgbcolor.rgb_b != rgbcolor.clamped_rgb_b:
            return False
        return True

    def run(self):
        self.setup_dictionary()
        maximum = int(1 / self.step)
        prog = 0
        bound = maximum * maximum * maximum
        for ri in range(maximum):
            r = ri*self.step
            for gi in range(maximum):
                g = gi*self.step
                for bi in range(maximum):
                    b = bi*self.step
                    rgbc = sRGBColor(r, g, b)
                    labc = convert_color(rgbc, LabColor)
                    dc = self.color_distance(labc, self.starting_point)
                    rdc = round(dc, 3)
                    if self.calculate_round_error(dc, rdc):
                        if rdc in self.values:
                            self.dictionary[rdc].append((round(rgbc.rgb_r, 3), round(rgbc.rgb_g, 3), round(rgbc.rgb_g, 3)))
                            print("Match! at {}                                  ".format(prog))
                    prog += 1
                    inline_out_of_progress(prog, bound)
        print("Done")
        name = "space_{}_{}_{}".format(self.spacing, self.step, self.diff)
        payload = []
        for val in self.values:
            payload.append(self.dictionary[val])
        result = {"Name": name,
                  "Labels": self.values,
                  "Payload": payload}
        with open("{}.json".format(name), "w+") as f:
            json.dump(result, f)
        exit()


def inline_out_of_progress(value, out_of, what=''):
    print(value.__str__() + "/" + out_of.__str__() + " " + what + " completed                  ", end='\r')
    sys.stdout.flush()


if __name__ == "__main__":
    app = ColorMapper()
    app.run()
