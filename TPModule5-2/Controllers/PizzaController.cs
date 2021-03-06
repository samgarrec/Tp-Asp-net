﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPModule5_2.Models;
using TPModule5_2.Utils;
using TPModule5_2_BO;

namespace TPModule5_2.Controllers
{
    public class PizzaController : Controller
    {
        // GET: Pizza
        public ActionResult Index()
        {
            return View(FakeDb.Instance.Pizzas);
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {

            
            PizzaViewModel vm = new PizzaViewModel();

            vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            return View(vm);
        }

        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(PizzaViewModel vm)
        {


            try
            {
                Pizza pizza = vm.Pizza;

                if (ModelState.IsValid && ValidateVM(vm))
                {
                    var isValidOrnot = true;

                    var nomPizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Nom == vm.Pizza.Nom);


                    if (nomPizza!=null)
                    {

                        isValidOrnot = false;
                     

                        //renvoi liste et pate dispo
                        ModelState.AddModelError("Pizza.Nom", "Nom de pizza en double");
                 
                    }

                                        pizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);
                    if (vm.Pates.Equals("select"))
                    {
                        isValidOrnot = false;
   //renvoi liste et pate dispo
                        ModelState.AddModelError("IdPate", "veuillez selectionner une pate");
                    }
                    pizza.Ingredients = FakeDb.Instance.IngredientsDisponible.Where(
                            x => vm.IdsIngredients.Contains(x.Id))
                            .ToList();
                    if (pizza.Ingredients.Count < 2 || pizza.Ingredients.Count>5)
                    {
                        isValidOrnot = false;

                        vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                      x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() }).ToList();

                        vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() }).ToList();

                        //renvoi liste et pate dispo
                        ModelState.AddModelError("IdsIngredients", "veuillez selectionner entre 2 et 5 ingredients");
                    }

                    // Insuffisant
                    //pizza.Id = FakeDb.Instance.Pizzas.Count + 1;



                    var isNotTheSame = IsDifferent(vm);



                    if (isValidOrnot && isNotTheSame)
                    {
                        pizza.Id = FakeDb.Instance.Pizzas.Count == 0 ? 1 : FakeDb.Instance.Pizzas.Max(x => x.Id) + 1;

                        FakeDb.Instance.Pizzas.Add(pizza);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                        x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                  .ToList();

                        vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                            x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                            .ToList();

                        return View(vm);


                    }

                }
                else
                {
                    vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

                    vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                        x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                        .ToList();

                    return View(vm);
                }
            }
            catch
            {
                vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

                vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                    x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                    .ToList();

                return View(vm);
            }
        }

       private bool ValidateVM(PizzaViewModel vm)
        {
            bool result = true;
            return result;
        }




















        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            PizzaViewModel vm = new PizzaViewModel();

            vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);

            if (vm.Pizza.Pate != null)
            {
                vm.IdPate = vm.Pizza.Pate.Id;
            }

            if (vm.Pizza.Ingredients.Any())
            {
                vm.IdsIngredients = vm.Pizza.Ingredients.Select(x => x.Id).ToList();
            }

            return View(vm);
        }




















        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(PizzaViewModel vm)
        {
            try
            {
                Pizza pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == vm.Pizza.Id);
                pizza.Nom = vm.Pizza.Nom;
                pizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);
                pizza.Ingredients = FakeDb.Instance.IngredientsDisponible.Where(x => vm.IdsIngredients.Contains(x.Id)).ToList();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            PizzaViewModel vm = new PizzaViewModel();
            vm.Pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
            return View(vm);
        }

        // POST: Pizza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Pizza pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
                FakeDb.Instance.Pizzas.Remove(pizza);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            PizzaViewModel vm = new PizzaViewModel();
            vm.Pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
            return View(vm);
        }
        public bool IsDifferent(PizzaViewModel vm)

        {

            bool isDifferent = true;



            Pizza pizzaViewModel = vm.Pizza;

            var listIngredients = vm.IdsIngredients.OrderBy(i => i);

            foreach (var pizza in FakeDb.Instance.Pizzas)

            {


                if (listIngredients.Count() != pizza.Ingredients.Count())

                {

                    return isDifferent;

                }

                else

                {

                    if (listIngredients.SequenceEqual(pizza.Ingredients.Select(i => i.Id).OrderBy(i => i)))

                    {
                        if (pizzaViewModel.Id == pizza.Id)

                        {

                            isDifferent = true;

                        }

                        else

                        {

                            isDifferent = false;

                            break;

                        }

                    }

                }



            }



            if (!isDifferent)

            {

                ModelState.AddModelError("IdsIngredients", ".Cette composition existe deja");

            }



            return isDifferent;

        }


    }
}
