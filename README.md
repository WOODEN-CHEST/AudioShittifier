# Audio shittifier
Do you ever hear a nice, high-quality song and think to yourself "the quality of this song is just TOO good"?
Well, worry no more, audio shittifier is here to save the day (if you get it working, that is).

## What does it do?
Audio shittifier simply takes in an audio file and a layout file, and then, using various effects defined in the layout file, turns the audio file into shit quality.

## How to use it?
The audio shittifier is a commandline program. Each argument is passed in a key-value fashion with the format <key>=<value> where the key is the name
of the argument and the value is the, well, value of it. Currently there are 4 arguments with two being mandatory:

### source
The "source" argument is a path to either a directory or a single file that you want to shittify.

### destination
The "destination" argument is a path to the destination directory where all the shittified files will be placed.
This argument may be omitted, the shittifier will create a default "out" directory in the source file's directory and place the output there.

### intensity
The "intensity" argument is a real number in the range [0;1] which describes the portion of effects in the layout which should be used. 
For example, a value of 1 will use all effects, a value of 0 will use no effects, a value of 0.5 will use half of the effects.

### layout
The "layout" argument is a path to a JSON file which contains the layout of the effects.

## Effect layouts, what are they?
To better customize how the audio is turned to garbage, there exist **effect layouts.**
A layout is a JSON file which describes which effects and with what arguments should be used when shittifying the file.

Each effect is defined in  a separate compound. The layout starts with either a single effect compound, or a list of effect compounds.

```
{}
```

OR

```
[
  {},
  {},
  {}
  ...
]
```


Each effect compound contains an entry with the key "name" and the value being the name of the effect.
```
[
  {
    "name": "resample"
  },
  {
    "name": "mono"
  },
  {
    "name": "precision"
  }
]
```

Afterwards, the effect compound simply contains the arguments for the effect. 
```
{
  "name": "resample",
  "parameter1": 57,
  "parameter2": "placeholder",
  "parameter3": 1.5
}
```
The names parameter1, 2 and 3 are just here for demonstration purposes, the actual parameter names are documented below.
In this example, all argument values were constant, but there are actually 3 ways to write arguments.
They can be either a **constant value**, **value range** or **selection value.**

A **constant** value is simply a single value, for example:
`"parameter": 50`
The parameter is set to the value 50, and that's it.

The **range value** is a number range with "best" and "worst" values.
The terms "best" and "worst" can be though of as min and max values. 
"best" describes the best value in terms of listening quality, and "worst" describes the value that most cripples the audio.
```
"parameter": {
  "best": 100,
  "worst": 500
}
```

```
"parameter": {
  "best": 1.0,
  "worst": 0.1
}
```

The final **selection value** argument type is a list of options.
The audio shittifier will randomly select one of the options.
All options in a selection value must be of the same type.

```
"parameter": [1, 5, 10, 20, 50, 100]
```
