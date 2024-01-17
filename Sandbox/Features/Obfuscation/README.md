A small experiment looking at ways of transforming a value to be used in a URL as a reference.

The requirements are:
- The cipher/encoded/hashed values should be reversible
- The transformed value should be, or could be made URL-friendly.
- (Obviously) Performance is important

Nice to have:
- The transformed value should be as short as possible
- The transformed value should be deterministic
- The transformed value shouldn't be considered secret, but hard to decode externally would be a bonus.