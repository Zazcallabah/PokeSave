var lookuptypes = ["Normal", "Fighting", "Flying", "Poison", "Ground", "Rock", "Bug", "Ghost", "Steel", "Unknown", "Fire", "Water", "Grass", "Electric", "Psychic", "Ice", "Dragon", "Dark", "Fairy"];
var TypeChart = function (tc) {
	this.Lines = [];
	for (var i = 0; i < tc.length; i++) {
		this.Lines.push(new Line(tc[i]));
	}
};
TypeChart.prototype.ForType = function (t) {
	for (var i = 0; i < this.Lines.length; i++) {
		if (this.Lines[i].Is(t))
			return this.Lines[i];
	}
};

var Line = function (tcl) {
	this.Type = [];
	this.TypeIndexes = tcl.t;
	this.Weights = [];
	for (var i = 0; i < tcl.t.length; i++) {
		this.Type.push(lookuptypes[tcl.t[i]]);
	}
	for (var i = 0; i < tcl.w.length; i++) {
		this.Weights.push(this._lookupweights[tcl.w[i]]);
	}
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

var TemplateData = function (e, typechart, level) {
	this.Name = e.Name;
	this.Types = e.Types;
	this.Attack = [];
	var line = typechart.ForType(e.Types);
	for (var i = 0; i < lookuptypes.length; i++) {
		var tmp = line.AttackedBy(lookuptypes[i]);
		if (tmp !== "x1" && tmp !== undefined) {
			this.Attack.push({
				Label:tmp,
				Class: tmp.replace('.',''),
				Type:lookuptypes[i]
			});
		}
	}
	this.MoveSet = [e.MoveSet[0], e.MoveSet[1], e.MoveSet[2], e.MoveSet[3]];
	this.FullMoveSet = e.MoveSet;
	var currentindex = 0;
	this.PossibleTypes = [];
	for (var i = 0; i < e.MoveSet.length; i++) {
		if (level !== undefined && e.MoveSet[i].Level <= level) {
			this.MoveSet[(currentindex++) % 4] = e.MoveSet[i];
		}
		var s = e.MoveSet[i].Type;
		if (!_IN(this.PossibleTypes, s)) {
			this.PossibleTypes.push(s);
		}
	}

	this.LevelTypes = [];
	if (level === undefined)
		return;
	for (var i = 0; i < this.MoveSet.length; i++) {
		if (!_IN(this.LevelTypes, this.MoveSet[i].Type))
			this.LevelTypes.push(this.MoveSet[i].Type);
	}
};

var Move = function (m) {
	this.Level = m.l;
	this.TypeIndex = m.t;
	this.Type = lookuptypes[m.t];
};
