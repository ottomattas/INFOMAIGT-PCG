# Terrain Generation Agent - Part 1


## Description

In the lecture, we discussed several approaches to generating terrains. In this assignment, you will design a system for generating a specific type of terrain of your choice (islands, mountain ranges, river deltas, etc). It is very important that you base your system on solid research, so you will need to find sources relating to the generation of the type of terrain you have chosen. You should write a document of at most three pages containing at least the following information:

The type of terrain you want to generate. Discuss the distinctive features of this type of terrain, and show some (real-world) examples.
A description of a system that will generate the given type of terrain. What is the input, and what is the output? What are the steps to generate the output from the given input? How will you ensure some randomness in the generated terrains?
A discussion of why the chosen method is suitable for the chosen type of terrain. Does your method generalise easily, or is it very specific?
A description of the parameters of your system, and the expected range of possible outputs.
The sources you are basing your design on.
Any libraries you plan on using. Depending on the type of system you want to implement, we may allow the use of libraries that take care of some of the smaller aspects (e.g. some structured noise generator library may be allowed if you want to add a small amount of texture to an already finished terrain).
Feel free to include some figures outside of the three-page limit.

It is worth keeping in mind that in the next assignment, you will have to implement the system you designed. Think about whether this will be feasible as you work on this assignment, keeping in mind that you will be allowed to use a visualisation tool of your choice, as long as a compiled version runs on Windows 10 without installing any libraries (e.g. Unity; we will also provide a simple Python tool for visualisation that you can use if you want). That said, your proposed system should have sufficient complexity; "a flat, featureless desert" is not an acceptable type of terrain to try to generate. You should also think about the expected performance of your system; don't choose a method that takes hours to generate something acceptable.



## Evaluation

Evaluation criteria are:

Suitability: the method you plan to use should be appropriate for generating the chosen type of terrain. This suitability should have some theoretical or empirical foundation, preferably from academic literature. This is the most important aspect of this assignment.
Detail: are all necessary details described? The document should give enough information to start on the implementation directly.
Realism: is the project realistically possible in the available time?
Ambition: is the project challenging enough? Both the type of terrain and the method for generating it should not be trivial. For instance, most types of fractal terrain generation will be too easy to implement.
Please feel free to ask questions in the lectures or on Teams if you are unsure about any aspects of the system you have in mind.

# Terrain Generation Agent - Part 2

## Description

In the previous assignment, you designed a system for generating a type of terrain of your choice, and you received feedback on the feasibility of your design. In this assignment, you are going to implement your system. You should submit your implementation (both source and a compiled version, if applicable), as well as a document containing the following information:

A description of how to use your program, including any libraries we need to install in case you're using Python.
Some examples of the results obtained, and the settings used to obtain them.
Any performance constraints; it should preferably take at most a few minutes to generate a terrain.
A description and discussion of the changes you had to make to your design, if applicable. Why were the changes necessary? Do they impact the quality and/or capabilities of your system?
You are provided with a simple Python program that gives you some basic visualisation tools, but you are free to use any system you want for visualisation (e.g. Unity). The only requirements are the following:

Your program needs to run on a Windows 10 machine. You can either provide a compiled file, or your Python code. We will not compile your program from C#/C++ code.
While you are free to use any library you want for visualisation of your result, you are not allowed to use libraries to implement part of your terrain generation, unless with our explicit permission. If you indicated you will use a library in your design document and we didn't comment on this, you have our permission. Please contact us if you are unsure! Also, don't assume you can use a particular library if we gave another group permission to use it!


## Using the code

We supply a simple program based on the Panda3D library that can give a simple rendering of a heightmap. It is not required to use this program, but don't spend a lot of time on getting nice renderings of your results. For the groups that will not use heightmaps, an option could be to write a commandline program that exports the result to a model that can be opened in Blender.

The program has a Renderer class that sets up some basic lighting. It also has a render_heightmap function that takes a numpy array with height values, the size of each grid cell, and an optional array of RGB colour values. The colour values should be in the range [0, 1], and the dtype of your numpy array should probably be something like (float, 3). If you don't supply a colour array, the terrain will simply be drawn in grey.



## Evaluation

You will primarily be evaluated on the quality of the terrain you generate, and the degree to which you were successful in implementing your proposed system. If there was a need to deviate from your original proposal, you will also be evaluated on the explanation of the reasons behind this deviation. "Not enough time" can be a valid reason if your proposal was quite ambitious!

We want to explicitly state that the minimum requirement for passing is definitely not a fully working implementation of your entire proposal. As each individual proposal offered a different level of challenge, it's hard to give explicit grading guidelines, but typically a fully working implementation will result in a much better grade than a six.
