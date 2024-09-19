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

If a parameter is not set in the layout, then the audio shittifier uses a default value. This means any and all parameters can be omitted if you do not wish to change them.
Multiple effects with the same type can be defined, allowing to have the same effect applied multiple times.


## Effect documentation.
Below you'll find the list of effects and their parameters.

### "biquadfilter"
Performs either a low frequency pass or high frequency pass operation on the audio.

"frequency" (integer) -The target frequency of the biquadfilter's operation

"type" (string) -The type of pass that is done, can be either "LowPass" or "HighPass"

"order" (integer) -The filter's order. Best not to set this.

### "clipping"
"Clips" the audio, making it cut out at random times.

"per_second" (real number) -The number of clips per second on average

"duration_min" (real number) -The minimum duratioin (in seconds) of a cutout.

"duration_max" (real number) -The maximum duratioin (in seconds) of a cutout.

### "echo"
Plays another instance of the audio over the current audio

"offset" (real number) -The offset of the played audio (in seconds).

"volume" (real number) -The volume multiplier of the played audio.

### "mono"
Averages all tracks in the audio.

### "multiplier"
Multiplies the audio samples by a value (changes the volume).

"amount" (real number) -The multiplier

### "noise"
Introduces random noise into the audio

"amount" (real number) -The portion of audio that should be random noise.

### "power"
Takes each sample of the audio to the given power. It makes it so that, if the "amount" parameter is set high, the peaks in audio become louder and the lows lower.
If set low, then the audio simply becomes louder.

"amount" (real number) -The power to which to take the samples, if set to 1 then nothing changes about the audio.

### "precision"
Changes the precision of the audio samples.

"steps" (integer) -The amount of steps that the sample may take from 0 to reach the maximum / minimum sample value.

### "repeat"
Repeats parts of the audio.

"per_second" (real number) -The number of repeats per second on average.

"duration_min" (real number) -The minimum duration (in seconds) of a repeated section.

"duration_max" (real number) -The maximum duration (in seconds) of a repeated section.

"count_min" (integer) -The minimum number of section copies made in a repeat.

"count_max" (integer) -The maximum number of section copies made in a repeat.

### "resample"
Resamples the audio at a different sample rate.

"rate" (integer) The target sample-rate to resample to.

### "swap"
Swaps sections of the audio.

"per_second" (real number) -The number of swaps per second on average.

"duration_min" (real number) -The minimum duration (in seconds) of a swapped section.

"duration_max" (real number) -The maximum duration (in seconds) of a swapped section.

### "waveform"
Replaces parts of the audio with constant waveforms into the audio.

"per_second" (real number) -The number of waveforms per second on average.

"duration_min" (real number) -The minimum duration (in seconds) of a waveform.

"duration_max" (real number) -The maximum duration (in seconds) of a waveform.

"type" (string) -The type of a waveform. May be "Sine", "Square", "Saw" or "Triangle"

"volume" (real number) -The volume of the waveform (multiplier)

"frequency_min" (real number) -The minimum frequency (Hz) of the waveform

"frequency_max" (real number) -The maximum frequency (Hz) of the waveform


## Some notes.
The program is fairly poorly made, so it can be buggy.
The order in which filters are applied depends on the order in which they are defined in the layout file.
The program is a memory hog, especially on larger audio files, may use up to 2GB of memory on normal length songs.
The program likely won't work on 30+ minute long audio files as those will use too much memory, sorry.



