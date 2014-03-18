var lookuptypes = ["Normal", "Fighting", "Flying", "Poison", "Ground", "Rock", "Bug", "Ghost", "Steel", "Unknown", "Fire", "Water", "Grass", "Electric", "Psychic", "Ice", "Dragon", "Dark", "Fairy"];
var TypeChart = function (tc) {
	this.Lines = [];
	var s_lines = [];
	for (var i = 0; i < tc.length; i++) {
		var l = new Line(tc[i]);
		this.Lines.push(l);
		s_lines.push(l);
	}
	for (var i = 0; i < s_lines.length; i++) {
		for (var j = i + 1; j < s_lines.length; j++) {
			this.Lines.push(new Line(s_lines[i], s_lines[j]));
		}
	}
};
TypeChart.prototype.ForType = function (t) {
	for (var i = 0; i < this.Lines.length; i++) {
		if (this.Lines[i].Is(t))
			return this.Lines[i];
	}
};
TypeChart.prototype.AttackWith = function (t) {
	var result = [];
	for (var i = 0; i < lookuptypes.length; i++) {
		if (i === 9) continue; // Unknown
		var line = this.ForType([lookuptypes[i]]);
		var weight = line.AttackedBy(t);
		if (weight !== "x1")
			result.push(new Attack(weight, lookuptypes[i]));
	}
	result.sort(asort);
	return result;
};

var Line = function (tcl, line) {
	if (line === undefined) {
		this.Type = [];
		this.WeightIndexes = tcl.w;
		this.Weights = [];
		for (var i = 0; i < tcl.t.length; i++) {
			this.Type.push(lookuptypes[tcl.t[i]]);
		}
		for (var i = 0; i < tcl.w.length; i++) {
			this.Weights.push(this._lookupweights[tcl.w[i]]);
		}
	} else {
		this.Type = [tcl.Type[0], line.Type[0]];
		this.Weights = [];
		this.WeightIndexes = [];
		for (var i = 0; i < tcl.WeightIndexes.length; i++) {
			var combined = combine(tcl.WeightIndexes[i], line.WeightIndexes[i]);
			this.Weights.push(this._lookupweights[combined]);
			this.WeightIndexes.push(combined);
		}
	}
};

var combine = function (x, y) {
	if (x === 0 || y === 0)
		return 0;
	if (x === 1)
		return y;
	if (y === 1)
		return x;
	if (x === 2 && y == 2)
		return 4;
	if (x === 5 && y === 5)
		return 8;
	return 1;
};
Line.prototype._lookupweights = ["x0", "x1", "x2", "", "x4", "x0.5", "", "", "x0.25"];
Line.prototype._lookupmap = {Normal:0, Fighting:6, Flying:9, Poison:7, Ground:8, Rock:12, Bug:11, Ghost:13, Steel:16, Unknown:-1, Fire:1, Water:2, Grass:4, Electric:3, Psychic:10, Ice:5, Dragon:14, Dark:15, Fairy:17};
Line.prototype.AttackedBy = function (t) {
	return this.Weights[this._lookupmap[t]];
};
Line.prototype.Is = function (t) {
	if (this.Type.length != t.length)
		return false;
	if (this.Type.length == 1)
		return this.Type[0] == t[0];

	return (this.Type[0] == t[0] && this.Type[1] == t[1] ) || (this.Type[0] == t[1] && this.Type[1] == t[0] );
};

var Dex = function (d) {
	this.Entries = [];
	this.Names = [];
	for (var i = 0; i < d.length; i++) {
		this.Entries.push(new Entry(d[i]));
		this.Names.push(d[i].n);
	}
};
Dex.prototype.Select = function (name, typechart, level) {
	for (var i = 0; i < this.Entries.length; i++) {
		if (this.Entries[i].Name === name) {
			return new TemplateData(this.Entries[i], typechart, level);
		}
	}
};

var Entry = function (e) {
	this.Name = e.n;
	this.TypeIndex = e.t;
	this.Types = [];
	for (var i = 0; i < e.t.length; i++) {
		this.Types.push(lookuptypes[e.t[i]]);
	}
	this.MoveSet = [];
	for (var i = 0; i < e.s.length; i++) {
		this.MoveSet.push(new Move(e.s[i]));
	}
};

function _IN(arr, s) {
	for (var i = 0; i < arr.length; i++) {
		if (arr[i] === s)
			return true;
	}
	return false;
}
var Attack = function (label, type) {
	this.Label = label;
	this.Class = label.replace('.', '');
	this.Type = type;
}
var asort = function(x,y)
{
return x.Label.localeCompare(y.Label)
}

var TemplateData = function (e, typechart, level) {
	this.Name = e.Name;
	this.Level = level;
	this.Types = e.Types;
	this.Attack = [];
	var line = typechart.ForType(e.Types);
	for (var i = 0; i < lookuptypes.length; i++) {
		var tmp = line.AttackedBy(lookuptypes[i]);
		if (tmp !== "x1" && tmp !== undefined) {
			this.Attack.push(new Attack(tmp, lookuptypes[i]));
		}
	}
	this.Attack.sort(asort);
	var l = e.MoveSet.length;
	this.MoveSet = [e.MoveSet[0%l], e.MoveSet[1%l], e.MoveSet[2%l], e.MoveSet[3%l]];
	this.FullMoveSet = e.MoveSet;
	this.LevelTypes = [];
	var currentindex = 0;
	this.PossibleTypes = [];
	var ptypes = [];

	for (var i = 0; i < e.MoveSet.length; i++) {
		if (level !== undefined && e.MoveSet[i].Level <= level) {
			this.MoveSet[(currentindex++) % 4] = e.MoveSet[i];
		}
		var s = e.MoveSet[i].Type;
		if (!_IN(ptypes, s)) {
			ptypes.push(s);
			this.PossibleTypes.push(new Stats(s, typechart));
		}
	}

	if (level === undefined || level === NaN)
		return;
	var levels = [];
	for (var i = 0; i < this.MoveSet.length; i++) {
		if (!_IN(levels, this.MoveSet[i].Type)) {
			levels.push(this.MoveSet[i].Type);
			this.LevelTypes.push(new Stats(this.MoveSet[i].Type, typechart));
		}
	}
};

var Stats = function (t, tc) {
	this.Type = t;
	this.Statistics = tc.AttackWith(t);
}

var Move = function (m) {
	this.Level = m[0];
	this.TypeIndex = m[1];
	this.Type = lookuptypes[m[1]];
};
