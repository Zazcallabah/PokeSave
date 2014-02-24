File definitions
================
This application can handle quite a few different files. The specifications and details of which can be found in this document. The application generally doesn't care about file endings, and will use file size, checksums or other tricks to try to figure out which file type has been opened.

Not everything in this file is implemented yet, but most things are.

.3gpkm
------
This file type is the de-facto standard for third gen pkm handling. It is basically the raw binary data for a pokemon found in the save file, but with a few key differences.

* The subsections are decrypted.
* The subsection order is G, A, E, M - i.e. the order they would be in if the personality was 0.
* As far as I can tell, they are all 80 bytes long.
* The pkm checksum should be correct.

The rest of the definition can be found on bulbapedia.

Raw pkm
-------
The raw, encrypted 80 or 100 bytes of a pokemon, lifted directly from a game save. To avoid confusion, this type cant be easily exported. (Enable fancy export in settings.)

Base64 encoded raw pokemon
--------------------------
This is just the Raw pkm format, but base64 encoded. Multiple entries can be in one file, separated by newlines.
You can label the entries in the file. Just separate your label from the base64 code with a colon.

    Example:VGhpcyBpcyBqdXN0IGFuIGV4YW1wbGU=
    label:QW5vdGhlciBleGFtcGxlLg==

The application will disregard everything before the last colon. The expected file encoding is utf-8, but unless you get really fancy with your labels that shouldn't be noticable.

Gen3 save file
--------------
Currently recognized by file size (128 KB).
Only tested with emulator saves from firered and leafgreen.