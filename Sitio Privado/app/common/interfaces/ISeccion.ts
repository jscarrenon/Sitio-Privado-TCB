module app.common.interfaces {

    export interface ISeccion {
        templates: string[];
        seccionURI: string;
        seccionId: number;
        seleccionarSeccion(id: number): void;
        setTemplates(): void;    
    }
}