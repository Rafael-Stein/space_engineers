bool recarregando;
float nivelBaterias;
List<IMyBatteryBlock> baterias = new List<IMyBatteryBlock>();
List<IMyPowerProducer> geradores = new List<IMyPowerProducer>();
List<IMyTextPanel> telas = new List<IMyTextPanel>();

public Program() {
    // The constructor, called only once every  session and
    // always before any other method is called. Use it to
    // initialize your script. 
    //     
    // The constructor is optional and can be removed if not
    // needed.
    // 
    // It's recommended to set RuntimeInfo.UpdateFrequency 
    // here, which will allow your script to run itself without a timer block.
    Runtime.UpdateFrequency = UpdateFrequency.Update100 | UpdateFrequency.Update100;

    //Intânciar todos os geradores e incluir na lista de geradores [List<IMyTerminalBlock> geradores]
    //Observação: Todos os geradores que serão controlados pelo script deverão conter "Gerador -" no seu nome
    //                     Painel Solar não precisa ligar/desligar para funcionar, por isto não necessita conter este nome
    //Exemplo: Gerador - Hydrogênio / Gerador - Reator
    //GridTerminalSystem.GetBlocksOfType<IMyPowerProducer>(geradores, b => b.BlockDefinition.ToString().ToLower().Contains("Gerador -"));
    //Descobrir como obter os componentes especificamente pelo nome
    GridTerminalSystem.GetBlocksOfType<IMyPowerProducer>(geradores);
    
    //Intânciar todos as baterias e incluir na lista de baterias [List<IMyBatteryBlock> baterias
    //Observação: Todas as baterias que serão controlados pelo script deverão conter "Bateria -" no seu nome
    //Exemplo: Bateria - 01 / Bateria - Principal / Bateria - Secundária
    //GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(baterias, c => c.BlockDefinition.ToString().ToLower().Contains("Bateria -"));
    //Descobrir como obter os componentes especificamente pelo nome
    GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(baterias);
    
    //Intânciar todos as telas de LCD e incluir na lista de telas [List<IMyTextPanel> baterias
    //Observação: Todas as baterias que serão controlados pelo script deverão conter "LCD -" no seu nome
    //Exemplo: Bateria - 01 / Bateria - Principal / Bateria - Secundária
    //GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(telas, c => c.BlockDefinition.ToString().ToLower().Contains("LCD -"));
    //Descobrir como obter os componentes especificamente pelo nome
    GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(telas);

}

public void Main(string argument, UpdateType updateSource) {
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.

    //Verificar o estado atual das baterias
    checarBaterias();

    //Ligar os geradores se o nível de carga das baterias for menor ou igual a 5% e as baterias não estiverem recarregando
    if(nivelBaterias <= 0.05f && !recarregando) {
        //Alterar o estado dos geradores (TRUE = gerando energia)
        estadoGeradores(true);
    }
    //Desligar os geradores se o nível de carga das baterias for maior que 95% e as baterias estiverem recarregando
    else if(nivelBaterias > 0.95f && recarregando) {
        //Alterar o estado dos geradores (TRUE = gerando energia)
        estadoGeradores(false);
    }

    //Mostrará na tela de todos os Painéis LCD
    for(int i = 0; i < telas.Count; i++) {
        telas[i].WriteText("Baterias " + (nivelBaterias * 100) + "%.");
    }

    //Mostrará na tela do Programmer Block
    Echo("Baterias em " + (nivelBaterias * 100) + "% da capacidade.");

}

//Verificar o nível médio das baterias em porcentagem
private void checarBaterias() {
    nivelBaterias = 0.0f;

    //Verificar se o total de energia acumulada é menor que o máximo de energia acumulada
    //if(vBatteries[i].CurrentStoredPower < (vBatteries[i].MaxStoredPower*0.01))

    //Verificar o acumulo de energia em cada bateria para saber o nível de energia acumulada
    for(int i = 0; i < baterias.Count; i++) {
        //Somar a quantidade de energia acumulada de cada bateria
        nivelBaterias += baterias[i].CurrentStoredPower;
    }

    //Dividir o nível das baterias pela quantidade de baterias para saber a porcentagem de energia acumulada
    nivelBaterias /= baterias.Count;

    Echo("Baterias em " + (nivelBaterias * 100) + "% da capacidade.");
}

//Ligar ou Desligar todos os geradores de energia
private void estadoGeradores(bool status) {
    
    for(int i = 0; i < geradores.Count; i++) {
    
        //Ligar o gerador caso ele não esteja ligado
        if(!geradores[i].Enabled) {
            geradores[i].Enabled = status;
            recarregando = status;
        }

    }

}

public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
}
