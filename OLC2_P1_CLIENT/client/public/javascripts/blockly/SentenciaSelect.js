var SelectFlag = false;

Blockly.Blocks['sentencia_select'] = {
    init: function() {
        this.appendStatementInput("select_block")
            .setCheck(null)
            .appendField("SELECT");
        this.appendDummyInput()
            .appendField("FROM")
            .appendField(new Blockly.FieldTextInput("Table"), "from_ins");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_select'] = function(block) {
    var statements_select_block = Blockly.JavaScript.statementToCode(block, 'select_block').split('%').filter(Boolean);
    var text_from_ins = block.getFieldValue('from_ins');
    // TODO: Assemble JavaScript into code variable.
    var code = 'SELECT '+ statements_select_block.join(", ").trim() +' FROM '+ text_from_ins + (block.getNextBlock() === null ? ';\n' : '\n');
    return code;
};