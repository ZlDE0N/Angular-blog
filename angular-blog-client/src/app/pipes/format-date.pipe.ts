import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatDate',
  pure: true, // Asegúrate de que sea puro
})
export class FormatDatePipe implements PipeTransform {
  transform(value: number): string {
    // Convierte el valor de milisegundos a una fecha legible
    const date = new Date(value);
    return date.toLocaleString(); // Ajusta según tus necesidades
  }
}
