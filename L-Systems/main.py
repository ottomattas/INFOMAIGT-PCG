#! /usr/bin/env python

from lindenmayer import LSystem, Rule, StochasticRule
from draw import TurtleDrawer

import math, sys

def flower():
    drawer = TurtleDrawer(4,1,16)

    lsystem = LSystem('X[B]C',
                      {
                          'X': 'DDDDDDDD',
                          'D': 'DFF',
                          # 'B': StochasticRule(((0.5, '+FB[-FB]'),(0.5, '-FB[+FB]'))),#'F+BF--FF+B',
                          'B': '[F+B--F+B]-B',
                          'C': 'L--L--L--L',
                          'L': 'S-S-S-S',
                          'S': '[A]-[A]-[A]-[A]-[A]-[A]',
                          'A': StochasticRule(((0.7, 'DDD[+D][-D]D'), (0.2, 'DDD[+D][--F]F'), (0.1, 'DDD[--D]F')))
                      })
    description = lsystem.evaluate(9)
    description2 = lsystem.evaluate(9)

    # uncomment if you want to see the string you're drawing
    # print(description)

    drawer.draw(description, offset=(200, -400))
    drawer.draw(description2, offset=(-200, -400), clear=False)
    drawer.done()

def tumbleweed():
    '''
    Draws a tumbleweed
    :return: A single image of a randomly generated tumbleweed
    '''
    drawer = TurtleDrawer(5, 1, 26)
    lsystem = LSystem('FFFX',
        {
            'F': StochasticRule(((0.5,'F-F[1+F]F[2-F]'),(0.3, 'F[1+FF]F[--F]'), (0.2,'FF[1++F]FF[2-F]'))),
            'X': 'F-F[3++F]F[1-F]'
        })
    description = lsystem.evaluate(4)

    # uncomment if you want to see the string you're drawing
    #print(description)

    drawer.draw(description, offset=(-100, 0))
    drawer.done()

#Refer to flowers/plants in Latin

if __name__ == '__main__':
    try:
        if sys.argv[1] == '1':
            flower()
        elif sys.argv[1] == '2':
            tumbleweed()
    except IndexError:
        print('Use any of the following numerical arguments to see one of the following plants:\n'
              '1: Dandelion\n'
              '2: Tumbleweed\n'
              '3: Tree')

