import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkillCardComponent } from './components/skill-card/skill-card.component';



@NgModule({
  declarations: [
    SkillCardComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    // Add other reusable components here if you want them to be available for use in other modules
    SkillCardComponent
  ]
})
export class SharedModule { }
