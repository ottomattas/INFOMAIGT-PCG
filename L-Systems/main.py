#! /usr/bin/env python

from lindenmayer import LSystem, Rule, StochasticRule
from draw import TurtleDrawer

import math

def main():
    drawer = TurtleDrawer(5, 1, 26)

    #STEEL= X, {X : XDD, D : DFF}
    #STAMPER=[+B][-B], {'B': '[F+BF--FF][F-BF++FF]'}
    #BLADEREN =DD[+F][-F]F
    # lsystem = LSystem('X[B]C',
    #     {
    #         'X': StochasticRule(((0.33,'DDDDDD-DD'),(0.33, 'DDDDDD+DD'), (0.33,'DDDDDDDD'))),
    #         'D': 'DFF',
    #         #'B': StochasticRule(((0.5, '+FB[-FB]'),(0.5, '-FB[+FB]'))),#'F+BF--FF+B',
    #         'B': '[F+B--F+B]-B',
    #         'C': 'L--L--L--L',
    #         'L': 'S-S-S-S',
    #         'S': '[A]--[A]--[A]--[A]--[A]--[A]',
    #         'A': StochasticRule(((0.7, 'DDD[+++DF][---DF]FF'), (0.2, 'DDD[+++DF][---F]FF'), (0.1, 'DDD[---DF]FF')))
    #     })
    lsystem = LSystem('FFFX',
        {
            'F': StochasticRule(((0.5,'F-F[1+F]F[2-F]'),(0.3, 'F[1+FF]F[--F]'), (0.2,'FF[1++F]FF[2-F]'))),
            'X': 'F-F[3++F]F[1-F]'
        })
    description = lsystem.evaluate(4)
    #description2 = lsystem.evaluate(9)

    # uncomment if you want to see the string you're drawing
    #print(description)

    drawer.draw(description, offset=(-200, 0))
    #drawer.draw(description2, offset=(-200, -400), clear= False)
    drawer.done()




if __name__ == '__main__':
    main()
